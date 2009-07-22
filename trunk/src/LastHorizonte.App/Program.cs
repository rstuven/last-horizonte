using System;
using System.IO;
using System.Security.Authentication;
using System.Threading;
using System.Windows.Forms;
using LastHorizonte.Core;

namespace LastHorizonte
{
	static class Program
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
		static void Main()
		{

			bool createdNew;
			var mutex = new Mutex(false, Application.ProductName + "/Instance", out createdNew);
			if (!createdNew)
			{
				MessageBox.Show("La aplicación ya está abierta.", 
					Application.ProductName,
				    MessageBoxButtons.OK, 
					MessageBoxIcon.Exclamation);
				return;
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(true);

			var optionsForm = new OptionsForm();

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

			Configuration = Configuration.Load(ConfigurationFilename);

			CreateHorizonteScrobbler(optionsForm);

			InitializeAndStartScrobbler(optionsForm, Configuration);

			Application.ApplicationExit += ((sender1, e) =>
			{
				notifyIcon.Visible = false;
				horizonteScrobbler.Stop();
			});

			Application.Run();

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
					if (track.IsError)
					{
						notifyIcon.ShowBalloonTip(0, Application.ProductName, track.Title, ToolTipIcon.Error);
						SetNotifyIconText("{0}: {1}", track.Status, track.Title);
					}
					else if (track.IsEmpty)
					{
						SetNotifyIconText("{0} esperando a que suene la mejor música...", Application.ProductName);
					}
					else
					{
						if (Configuration.NotifySystemTray)
						{
							notifyIcon.ShowBalloonTip(0, track.Status, track.ToString(), ToolTipIcon.Info);
						}
						SetNotifyIconText("{0}: {1}", track.Status, track.ToString());
					}
				}
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