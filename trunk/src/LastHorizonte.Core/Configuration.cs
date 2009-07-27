using System;
using System.IO;
using System.Xml.Serialization;

namespace LastHorizonte.Core
{
	[Serializable]
	public class Configuration
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public bool RememberPassword { get; set; }
		public bool StartActivated { get; set; }
		public bool StartOnWindowsSession { get; set; }
		public bool NotifyLastFm { get; set; }
		public bool NotifySystemTray { get; set; }
		public bool NotifyMsnMessenger { get; set; }

		public bool IsWindows
		{
			get
			{
				var platform = Environment.OSVersion.Platform;
				return
					platform == PlatformID.Win32Windows ||
					platform == PlatformID.Win32S ||
					platform == PlatformID.Win32NT ||
					platform == PlatformID.WinCE;
			}
		}

		public void SetPassword(string password)
		{
			Password = Lastfm.Utilities.md5(password);
		}

		public Configuration()
		{
			RememberPassword = true;
			StartActivated = true;
			StartOnWindowsSession = IsWindows;
			NotifyLastFm = true;
			NotifyMsnMessenger = IsWindows;
			NotifySystemTray = true;
		}

		public static Configuration Load(string filename)
		{
			if (File.Exists(filename))
			{
				Configuration configuration;
				using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
				{
					var serializer = new XmlSerializer(typeof(Configuration));
					configuration = (Configuration)serializer.Deserialize(stream);
				}
				return configuration;
			}
			return  new Configuration();
		}

		public void Save(String filename)
		{
			var password = this.Password;
			if (!this.RememberPassword)
			{
				// Temporarily "forget" password
				this.Password = "";
			}

			var serializer = new XmlSerializer(typeof(Configuration));
			using (var writer = new StreamWriter(filename))
			{
				serializer.Serialize(writer, this);
			}

			this.Password = password;
		}
	}
}