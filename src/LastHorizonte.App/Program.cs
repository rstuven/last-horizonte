using System;
using System.Diagnostics;
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
		private static HorizonteScrobbler horizonteScrobbler;
		private static IApplicationPresenter application;

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
			if (Configuration.IsRunningOnMono)
			{
				application = new GtkApplicationPresenter();
			}
			else
			{
				application = new WinFormsApplicationPresenter();
			}

			application.Initialize();

			if (!Configuration.IsRunningOnMono)
			{
				CheckUpdate(false);
			}

			Configuration = Configuration.Load(ConfigurationFilename);


			application.CreateNotifyIcon(ContextMenu.GetItems(application), "Iniciando sesión...");
			CreateHorizonteScrobbler();
			InitializeAndStartScrobbler(Configuration);

			application.Start(horizonteScrobbler);
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
					//CheckSingleInstance();
				};
			}

			updater.TryUpdate();
		}

		internal static void InitializeAndStartScrobbler(Configuration configuration)
		{
			try
			{
				horizonteScrobbler.Start(configuration);
			}
			catch (AuthenticationException)
			{
				application.OpenAuthentication();
			}
			catch(Exception ex)
			{
				application.ShowBalloonTipError(Application.ProductName, ex.Message);
			}
		}

		internal static void CreateHorizonteScrobbler()
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
						application.ShowBalloonTipError(null, track.Title);
						application.SetNotifyIconText("{0}: {1}", "Error", track.Title);
					}
					else if (track.Status == TrackStatus.None)
					{
						application.SetNotifyIconText("{0} esperando a que suene la mejor música...", Application.ProductName);
					}
					else
					{
						var status = (track.Status == TrackStatus.Coming ? "Luego en" : "En") + " Horizonte";
						if (Configuration.NotifySystemTray)
						{
							application.ShowBalloonTipInfo(status, track.ToString());
						}
						application.SetNotifyIconText("{0}: {1}", status, track.ToString());
					}
				}
			});
			horizonteScrobbler.Loved += ((sender, eventargs) =>
			{
				application.ShowBalloonTipInfo("Favorito", eventargs.Track.ToString());
			});
			horizonteScrobbler.Banned += ((sender, eventargs) => 
			{
				application.ShowBalloonTipInfo("Vetado", eventargs.Track.ToString());
			});
			horizonteScrobbler.Started += ((sender, eventargs) =>
			{
				application.ShowBalloonTipInfo(Application.ProductName, "Activado");
			});
			horizonteScrobbler.Stopped += ((sender, eventargs) =>
			{
				application.ShowBalloonTipInfo(Application.ProductName, "Desactivado");
			});
		}

		public static void OpenLastFmSignup()
		{
			Process.Start("http://www.lastfm.es/join");
		}
	}
}