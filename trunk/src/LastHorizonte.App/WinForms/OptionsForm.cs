using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using LastHorizonte.Core;

namespace LastHorizonte
{
	public partial class OptionsForm : Form
	{
		public OptionsForm()
		{
			InitializeComponent();

			this.Text = Application.ProductName + " - Opciones";
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			if (WindowState == FormWindowState.Minimized)
			{
				this.Hide();
			}
			base.OnSizeChanged(e);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			e.Cancel = true;
			this.Hide();
		}

		private bool accountChanged;

		private void acceptButton_Click(object sender, EventArgs e)
		{
			this.Hide();

			SaveOptions();

			this.Close();
		}

		private void SaveOptions()
		{
			var cfg = Program.Configuration;
			if (accountChanged)
			{
				cfg.Username = usernameTextBox.Text;
				cfg.SetPassword(passwordTextBox.Text);
			}
			if (accountChanged)
			{
				Program.CreateHorizonteScrobbler();
				Program.InitializeAndStartScrobbler(cfg);
			}

			cfg.RememberPassword = rememberPasswordCheckBox.Checked;
			cfg.NotifyLastFm = notifyLastFmCheckBox.Checked;
			cfg.NotifyMsnMessenger = notifyMsnMessegerCheckBox.Checked;
			cfg.NotifySystemTray = notifySystemTrayCheckBox.Checked;
			cfg.StartActivated = startActivatedCheckBox.Checked;
			cfg.StartOnWindowsSession = startOnWindowsSessionCheckBox.Checked;

			if (Configuration.IsRunningOnWindows)
			{
				try
				{
					var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
					if (cfg.StartOnWindowsSession)
					{
						// Use app domain setup information to get launcher path
						var domainSetup = AppDomain.CurrentDomain.SetupInformation;
						key.SetValue(Application.ProductName, domainSetup.ApplicationBase + domainSetup.ApplicationName);
					}
					else
					{
						key.DeleteValue(Application.ProductName, false);
					}
				}
				catch (Exception)
				{
					cfg.StartOnWindowsSession = false;
				}
			}

			if (activatedCheckBox.Checked != Program.HorizonteScrobbler.IsStarted)
			{
				if (activatedCheckBox.Checked)
				{
					Program.InitializeAndStartScrobbler(cfg);
				}
				else
				{
					Program.HorizonteScrobbler.Stop();
				}
			}

			cfg.Save(Program.ConfigurationFilename);
		}

		public void OpenWithAuthenticationError()
		{
			Open();

			errorProvider.SetError(usernameTextBox, "Ingrese un nombre de usuario correcto");
			errorProvider.SetError(passwordTextBox, "Ingrese una contrasela correcta");

			if (Program.Configuration.StartActivated)
			{
				activatedCheckBox.Checked = true;
			}
		}

		public void Open()
		{
			errorProvider.Clear();

			LoadOptions();

			Application.DoEvents();
			accountChanged = false;

			this.Show();
			this.WindowState = FormWindowState.Normal;

			this.Activate();
			usernameTextBox.Focus();
		}

		private void LoadOptions()
		{
			var cfg = Program.Configuration;
			usernameTextBox.Text = cfg.Username;
			passwordTextBox.Text = cfg.Password;
			rememberPasswordCheckBox.Checked = cfg.RememberPassword;
			notifyLastFmCheckBox.Checked = cfg.NotifyLastFm;
			notifyMsnMessegerCheckBox.Checked = cfg.NotifyMsnMessenger;
			notifyMsnMessegerCheckBox.Enabled = Configuration.IsRunningOnWindows;
			notifySystemTrayCheckBox.Checked = cfg.NotifySystemTray;
			notifySystemTrayCheckBox.Enabled = !Configuration.IsRunningOnMono;
			startActivatedCheckBox.Checked = cfg.StartActivated;
			startOnWindowsSessionCheckBox.Checked = cfg.StartOnWindowsSession;
			startOnWindowsSessionCheckBox.Enabled = Configuration.IsRunningOnWindows;
			activatedCheckBox.Checked = Program.HorizonteScrobbler.IsStarted;
		}

		private void usernameTextBox_TextChanged(object sender, EventArgs e)
		{
			accountChanged = true;
		}

		private void passwordTextBox_TextChanged(object sender, EventArgs e)
		{
			accountChanged = true;
		}

		private void signupLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Program.OpenLastFmSignup();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

	}
}