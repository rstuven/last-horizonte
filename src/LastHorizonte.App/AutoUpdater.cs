/*
 * AutoUpdater.cs
 * This class is the main component of the AutoUpdater
 *  
 * Copyright 2004 Conversive, Inc.
 * 
 */

/*
 * Conversive's C# AutoUpdater Component
 * Copyright 2004 Conversive, Inc.
 * 
 * This is a component which allows automatic software updates.
 * It is written in C# and was developed by Conversive, Inc. on April 14th 2004.
 * 
 * The C# AutoUpdater Component is licensed under the LGPL:
 * ------------------------------------------------------------------------
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 * 
 * ------------------------------------------------------------------------
 */

using System;
using System.IO;
using System.Net.Security;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using System.Net;
using ICSharpCode.SharpZipLib.Zip;

namespace Conversive.AutoUpdater
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class AutoUpdater : Component
	{
		//Added 11/16/2004 For Proxy Clients, Thanks George for submitting these changes
		[DefaultValue(false)]
		[Description("Set to True if you want to use http proxy."),
		 Category("AutoUpdater Configuration")]
		public bool ProxyEnabled { get; set; }

		//Added 11/16/2004 For Proxy Clients, Thanks George for submitting these changes
		[DefaultValue(@"http://myproxy.com:8080/")]
		[Description("The Proxy server URL.(For example:http://myproxy.com:port)"),
		 Category("AutoUpdater Configuration")]
		public string ProxyUrl { get; set; }

		[DefaultValue(@"")]
		[Description("The UserName to authenticate with."),
		 Category("AutoUpdater Configuration")]
		public string LoginUserName { get; set; }

		[DefaultValue(@"")]
		[Description("The Password to authenticate with."),
		 Category("AutoUpdater Configuration")]
		public string LoginUserPass { get; set; }

		[DefaultValue(@"http://localhost/UpdateConfig.xml")]
		[Description("The URL Path to the configuration file."),
		 Category("AutoUpdater Configuration")]
		public string ConfigUrl { get; set; }

		[DefaultValue(true)]
		[Description("Set to True if you want the app to Restart automatically, set to False if you want to use the DownloadForm to prompt the user, if AutoDownload is false and DownloadForm is null, the app will not download the latest version."),
		 Category("AutoUpdater Configuration")]
		public bool AutoDownload { get; set; }

		public Form DownloadForm { get; set; }

		[DefaultValue(false)]
		[Description("Set to True if you want the app to Restart automatically, set to False if you want to use the RestartForm to prompt the user, if AutoRestart is false and RestartForm is null, the app will not Restart."),
		 Category("AutoUpdater Configuration")]
		public bool AutoRestart { get; set; }

		public Form RestartForm { get; set; }

		[BrowsableAttribute(false)]
		public string LatestConfigChanges
		{
			get
			{
				string stRet = null;
				//Protect against NPE's
				if (this.AutoUpdateConfig != null)
				{
					stRet = AutoUpdateConfig.LatestChanges;
				}
				return stRet;
			}
		}

		[BrowsableAttribute(false)]
		public Version CurrentAppVersion
		{ get { return Assembly.GetEntryAssembly().GetName().Version; } }

		[BrowsableAttribute(false)]
		public Version LatestConfigVersion
		{
			get
			{
				Version versionRet = null;
				//Protect against NPE's
				if (this.AutoUpdateConfig != null)
				{
					versionRet = new Version(this.AutoUpdateConfig.AvailableVersion);
				}
				return versionRet;
			}
		}

		[BrowsableAttribute(false)]
		public bool NewVersionAvailable
		{ get { return this.LatestConfigVersion > this.CurrentAppVersion; } }

		[BrowsableAttribute(false)]
		public AutoUpdateConfig AutoUpdateConfig { get; private set; }

		public delegate void ConfigFileDownloaded(bool bNewVersionAvailable);
		public event ConfigFileDownloaded OnConfigFileDownloaded;

		public delegate void AutoUpdateComplete();
		public event AutoUpdateComplete OnAutoUpdateComplete;

		public delegate void AutoUpdateError(string stMessage, Exception e);
		public event AutoUpdateError OnAutoUpdateError;

		/// <summary>
		/// TryUpdate: Invoke this method if you just want to load the config without autoupdating
		/// </summary>
		public void LoadConfig()
		{
			Thread backgroundLoadConfigThread = new Thread(this.LoadConfigThread);
			backgroundLoadConfigThread.IsBackground = true;
			backgroundLoadConfigThread.Start();
		}//TryUpdate()

		/// <summary>
		/// loadConfig: This method just loads the config file so the app can check the versions manually
		/// </summary>
		private void LoadConfigThread()
		{
			var config = new AutoUpdateConfig();
			config.OnLoadConfigError += config_OnLoadConfigError;

			//For using untrusted SSL Certificates
			ServicePointManager.ServerCertificateValidationCallback =
				new RemoteCertificateValidationCallback(
					(sender, certificate, chain, sslPolicyErrors) => true);

			//Do the load of the config file
			if (config.LoadConfig(this.ConfigUrl, this.LoginUserName, this.LoginUserPass, this.ProxyUrl, this.ProxyEnabled))
			{
				this.AutoUpdateConfig = config;
				if (this.OnConfigFileDownloaded != null)
				{
					this.OnConfigFileDownloaded(this.NewVersionAvailable);
				}
			}
			//else
			//	MessageBox.Show("Problem loading config file, from: " + this.ConfigUrl);
		}

		/// <summary>
		/// TryUpdate: Invoke this method when you are ready to run the update checking thread
		/// </summary>
		public void TryUpdate()
		{
			var backgroundThread = new Thread(this.UpdateThread);
			backgroundThread.IsBackground = true;
			backgroundThread.Start();
		}//TryUpdate()		

		/// <summary>
		/// UpdateThread: This is the Thread that runs for checking updates against the config file
		/// </summary>
		private void UpdateThread()
		{
			const string updateName = "update";
			if (this.AutoUpdateConfig == null)//if we haven't already downloaded the config file, do so now
			{
				this.LoadConfigThread();
			}
			if (this.AutoUpdateConfig != null)//make sure we were able to download it
			{
				//Check the file for an update
				if (this.LatestConfigVersion > this.CurrentAppVersion)
				{
					//Download file if the user requests or AutoDownload is True
					if (this.AutoDownload || (this.DownloadForm != null && this.DownloadForm.ShowDialog() == DialogResult.Yes))
					{
						//MessageBox.Show("New Version Available, New Version: " + vConfig.ToString() + "\r\nDownloading File from: " + config.AppFileUrl);
						//var directoryInfo = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
						var directoryInfo = new DirectoryInfo(Application.StartupPath);
						var zipFilename = Path.Combine(directoryInfo.FullName, updateName + ".zip");
						//There is a new version available
						if (this.DownloadFile(this.AutoUpdateConfig.AppFileUrl, zipFilename))
						{
							var destination = directoryInfo.FullName;
							Unzip(zipFilename, destination);
							File.Delete(zipFilename);
							if (this.OnAutoUpdateComplete != null)
							{
								this.OnAutoUpdateComplete();
							}
							//Restart App if Necessary
							//If true, the app will Restart automatically, if false the app will use the RestartForm to prompt the user, if RestartForm is null, it doesn't Restart
							if (this.AutoRestart || (this.RestartForm != null && this.RestartForm.ShowDialog() == DialogResult.Yes))
							{
								Restart();
							}
							//else don't Restart
						}
						//else
						//	MessageBox.Show("Didn't Download File");
					}

				}
				//else
				//	MessageBox.Show("No New Version Available, Web Version: " + vConfig.ToString() + ", Current Version: " +  vCurrent.ToString());
			}

		}//UpdateThread()

		/// <summary>
		/// DownloadFile: Download a file from the specified url and copy it to the specified path
		/// </summary>
		private bool DownloadFile(string url, string path)
		{
			try
			{
				//create web request/response

				var request = WebRequest.Create(url);
				//Request.Headers.Add("Translate: f"); //Commented out 11/16/2004 Matt Palmerlee, this Header is more for DAV and causes a known security issue
				if (!string.IsNullOrEmpty(this.LoginUserName))
				{
					request.Credentials = new NetworkCredential(this.LoginUserName, this.LoginUserPass);
				}
				else
				{
					request.Credentials = CredentialCache.DefaultCredentials;
				}

				//Added 11/16/2004 For Proxy Clients, Thanks George for submitting these changes
				if (this.ProxyEnabled)
				{
					request.Proxy = new WebProxy(this.ProxyUrl);
				}

				using (var response = request.GetResponse())
				{
					using (var respStream = response.GetResponseStream())
					{
						var buffer = new byte[4096];

						using (var fs = File.Open(path, FileMode.Create, FileAccess.Write))
						{
							var length = respStream.Read(buffer, 0, 4096);
							while (length > 0)
							{
								fs.Write(buffer, 0, length);
								length = respStream.Read(buffer, 0, 4096);
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				var stMessage = "Problem downloading and copying file from: " + url + " to: " + path;
				//MessageBox.Show(stMessage);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				this.sendAutoUpdateError(stMessage, e);
				return false;
			}
			return true;
		}//DownloadFile(string url, string path)

		/// <summary>
		/// Unzip: Open the zip file specified by stZipPath, into the stDestPath Directory
		/// </summary>
		private static void Unzip(string stZipPath, string stDestPath)
		{
			using (var s = new ZipInputStream(File.OpenRead(stZipPath)))
			{
				ZipEntry theEntry;
				while ((theEntry = s.GetNextEntry()) != null)
				{

					var fileName = stDestPath + Path.GetDirectoryName(theEntry.Name) + Path.DirectorySeparatorChar +
								   Path.GetFileName(theEntry.Name);

					//create directory for file (if necessary)
					Directory.CreateDirectory(Path.GetDirectoryName(fileName));

					if (!theEntry.IsDirectory)
					{
						try
						{
							using (var streamWriter = File.Create(fileName))
							{
								var data = new byte[2048];
								try
								{
									while (true)
									{
										var size = s.Read(data, 0, data.Length);
										if (size > 0)
										{
											streamWriter.Write(data, 0, size);
										}
										else
										{
											break;
										}
									}
								}
								catch
								{
								}
							}
						}
						catch (Exception)
						{
							//MessageBox.Show("", "!!!!");
							//System.Diagnostics.Debug.Assert(false);
						}
					}
				}
			}
		}//Unzip(string stZipPath, string stDestPath)

		/// <summary>
		/// Restart: Restart the app, the AppStarter will be responsible for actually restarting the main application.
		/// </summary>
		private static void Restart()
		{
			Environment.ExitCode = 2; //the surrounding AppStarter must look for this to Restart the app.
			Application.Exit();
		}//Restart()

		private void config_OnLoadConfigError(string stMessage, Exception e)
		{
			this.sendAutoUpdateError(stMessage, e);
		}

		private void sendAutoUpdateError(string stMessage, Exception e)
		{
			if (this.OnAutoUpdateError != null)
			{
				this.OnAutoUpdateError(stMessage, e);
			}
		}
	}//class AutoUpdater

}//namespace Conversive.AutoUpdater
