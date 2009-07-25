/*
 * AutoUpdateConfig.cs
 * This class is the definition of the remote XML configuration file
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
 *  * 
 * ------------------------------------------------------------------------
 */

using System;
using System.Xml;
using System.Net;
using System.Diagnostics;

namespace Conversive.AutoUpdater
{
	/// <summary>
	/// Summary description for AutoUpdateConfig.
	/// </summary>
	public class AutoUpdateConfig
	{
		public string AvailableVersion { get; set; }
		public string AppFileUrl { get; set; }
		public string LatestChanges { get; set; }
		public string ChangeLogUrl { get; set; }

		public delegate void LoadConfigError(string message, Exception e);
		public event LoadConfigError OnLoadConfigError;

		/// <summary>
		/// LoadConfig: Invoke this method when you are ready to populate this object
		/// </summary>
		public bool LoadConfig(string url, string user, string pass, string proxyURL, bool proxyEnabled)
		{
			try 
			{
				//Load the xml config file
				var xmlDoc = new XmlDocument();
				//Retrieve the File

				var request = WebRequest.Create(url);
				//Request.Headers.Add("Translate: f"); //Commented out 11/16/2004 Matt Palmerlee, this Header is more for DAV and causes a known security issue
				if(!string.IsNullOrEmpty(user))
				{
					request.Credentials = new NetworkCredential(user, pass);
				}
				else
				{
					request.Credentials = CredentialCache.DefaultCredentials;
				}

				//Added 11/16/2004 For Proxy Clients, Thanks George for submitting these changes
				if(proxyEnabled)
				{
					request.Proxy = new WebProxy(proxyURL,true);
				}

				using (var response = request.GetResponse())
				{
					using (var respStream = response.GetResponseStream())
					{
						//Load the XML from the stream
						xmlDoc.Load(respStream);
					}
				}

				//Parse out the AvailableVersion
				var availableVersionNode = xmlDoc.SelectSingleNode(@"//AvailableVersion");
				this.AvailableVersion = availableVersionNode.InnerText;

				//Parse out the AppFileUrl
				var appFileURLNode = xmlDoc.SelectSingleNode(@"//AppFileUrl");
				this.AppFileUrl = appFileURLNode.InnerText;

				//Parse out the LatestChanges
				var latestChangesNode = xmlDoc.SelectSingleNode(@"//LatestChanges");
				if(latestChangesNode != null)
				{
					this.LatestChanges = latestChangesNode.InnerText;
				}
				else
				{
					this.LatestChanges = "";
				}

				//Parse out the ChangLogURL
				var changeLogUrlNode = xmlDoc.SelectSingleNode(@"//ChangeLogUrl");
				if(changeLogUrlNode != null)
				{
					this.ChangeLogUrl = changeLogUrlNode.InnerText;
				}
				else
				{
					this.ChangeLogUrl = "";
				}
			
			} 
			catch (Exception e)
			{
				var stMessage = "Failed to read the config file at: " + url + "\r\nMake sure that the config file is present and has a valid format.";
				Debug.WriteLine(stMessage); 
				//MessageBox.Show(stMessage); 
				if(this.OnLoadConfigError != null)
				{
					this.OnLoadConfigError(stMessage, e);
				}

				return false;
			}
			return true;
		}//LoadConfig(string url, string user, string pass)


	}//class AutoUpdateConfig
}//namespace Conversive.AutoUpdater
