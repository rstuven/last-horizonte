using System;
using System.IO;
using System.Security.Authentication;
using System.Threading;
using System.Windows.Forms;
using Conversive.AutoUpdater;
using LastHorizonte.Core;

namespace LastHorizonte
{
	public static class Program
	{
		internal static readonly string ConfigurationFilename = Path.Combine(Application.UserAppDataPath, "config.xml");
		public static Configuration Configuration;
		private static NotifyIcon notifyIcon;
		private static HorizonteScrobbler horizonteScrobbler;

		public static HorizonteScrobbler HorizonteScrobbler
		{
			get
			{
				return horizonteScrobbler;
			}
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(true);

			CheckUpdate(true);

			Configuration = Configuration.Load(ConfigurationFilename);

			var optionsForm = new OptionsForm();

			CreateNotifyIcon(optionsForm);
			CreateHorizonteScrobbler(optionsForm);
			InitializeAndStartScrobbler(optionsForm, Configuration);

			Application.ApplicationExit += ((sender1, e) =>
			{
				notifyIcon.Visible = false;
				horizonteScrobbler.Stop();
			});

			Application.Run();

		}

		private static void CreateNotifyIcon(OptionsForm optionsForm)
		{
			notifyIcon = new NotifyIcon
			{
				Icon = optionsForm.Icon,
				Visible = true,
				ContextMenuStrip = optionsForm.contextMenuStrip,
				Text = "Iniciando sesión..."
			};
			notifyIcon.ShowBalloonTip(0, Application.ProductName, notifyIcon.Text, ToolTipIcon.Info);

			notifyIcon.DoubleClick +=
				delegate
				{
					if (optionsForm.Visible)
					{
						optionsForm.Hide();
					}
					else
					{
						optionsForm.Open();
					}
				};
		}

		internal static void CheckUpdate(bool checkSingleInstance)
		{
			var updater = new AutoUpdater();
			updater.ConfigUrl = "http://last-horizonte.googlecode.com/files/UpdateVersion.xml?nocache=" + DateTime.UtcNow.ToString("yyyyMMddHHmmss");
			updater.DownloadForm = new UpdateForm(updater);
			updater.OnAutoUpdateComplete += delegate
			{
				Thread.Sleep(5000);
				//Application.Restart();
			};
			updater.AutoRestart = true;

			if (checkSingleInstance)
			{
				updater.OnConfigFileDownloaded += delegate
				{
					CheckSingleInstance();
				};
			}

			updater.TryUpdate();
		}

		private static void CheckSingleInstance()
		{
			bool createdNew;
			var mutex = new Mutex(true, Application.ProductName + "_Instance", out createdNew);
			if (!createdNew)
			{
				MessageBox.Show("La aplicación ya está abierta.",
				                Application.ProductName,
				                MessageBoxButtons.OK,
				                MessageBoxIcon.Exclamation);
				Application.Exit();
			}
		}

		internal static void InitializeAndStartScrobbler(OptionsForm optionsForm, Configuration configuration)
		{
			try
			{
				horizonteScrobbler.Start(configuration);
			}
			catch (AuthenticationException)
			{
				optionsForm.OpenWithAuthenticationError();
			}
			catch(Exception ex)
			{
				notifyIcon.ShowBalloonTip(5000, Application.ProductName, ex.Message, ToolTipIcon.Error);
			}
		}

		internal static void CreateHorizonteScrobbler(OptionsForm optionsForm)
		{
			if (horizonteScrobbler != null)
			{
				horizonteScrobbler.Stop();
			}
			horizonteScrobbler = new HorizonteScrobbler();
			horizonteScrobbler.Playing += ((sender, eventargs) =>
			{
				var track = eventargs.Track;
				if (eventargs.IsFirstTime)
				{
					if (track.Status == TrackStatus.Error)
					{
						notifyIcon.ShowBalloonTip(0, Application.ProductName, track.Title, ToolTipIcon.Error);
						SetNotifyIconText("{0}: {1}", "Error", track.Title);
					}
					else if (track.Status == TrackStatus.None)
					{
						SetNotifyIconText("{0} esperando a que suene la mejor música...", Application.ProductName);
					}
					else
					{
						var status = (track.Status == TrackStatus.Coming ? "Luego en" : "En") + " Horizonte";
						if (Configuration.NotifySystemTray)
						{
							notifyIcon.ShowBalloonTip(0, status, track.ToString(), ToolTipIcon.Info);
						}
						SetNotifyIconText("{0}: {1}", status, track.ToString());
					}
				}
			});
			horizonteScrobbler.Loved += ((sender, eventargs) => 
			{
				notifyIcon.ShowBalloonTip(0, "Favorito", eventargs.Track.ToString(), ToolTipIcon.Info);
			});
			horizonteScrobbler.Banned += ((sender, eventargs) => 
			{
				notifyIcon.ShowBalloonTip(0, "Vetado", eventargs.Track.ToString(), ToolTipIcon.Info);
			});
			horizonteScrobbler.Started += ((sender, eventargs) =>
			{
				notifyIcon.ShowBalloonTip(0, Application.ProductName, "Activado", ToolTipIcon.Info);
			});
			horizonteScrobbler.Stopped += ((sender, eventargs) =>
			{
				notifyIcon.ShowBalloonTip(0, Application.ProductName, "Desactivado", ToolTipIcon.Info);
			});
		}

		private static void SetNotifyIconText(string format, params object[] args)
		{
			var text = String.Format(format, args);
			// Maxmimun length supported by NotifyIcon.Text is 63 characters.
			if (text.Length > 63)
			{
				text = text.Substring(0, 60) + "...";
			}
			notifyIcon.Text = text;
		}
	}
}