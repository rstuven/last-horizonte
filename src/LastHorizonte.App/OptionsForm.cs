using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

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

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void activatedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (activatedToolStripMenuItem.Checked)
			{
				Program.InitializeAndStartScrobbler(this, Program.Configuration);
			}
			else
			{
				Program.HorizonteScrobbler.Stop();
			}
		}

		private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			activatedToolStripMenuItem.Checked = Program.HorizonteScrobbler.IsStarted;
			openProfileToolStripMenuItem.Enabled = Program.HorizonteScrobbler.IsInitialized;
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
				Program.CreateHorizonteScrobbler(this);
				Program.InitializeAndStartScrobbler(this, cfg);
			}

			cfg.RememberPassword = rememberPasswordCheckBox.Checked;
			cfg.NotifyLastFm = notifyLastFmCheckBox.Checked;
			cfg.NotifyMsnMessenger = notifyMsnMessegerCheckBox.Checked;
			cfg.NotifySystemTray = notifySystemTrayCheckBox.Checked;
			cfg.StartActivated = startActivatedCheckBox.Checked;

			if (activatedCheckBox.Checked != Program.HorizonteScrobbler.IsStarted)
			{
				if (activatedCheckBox.Checked)
				{
					Program.InitializeAndStartScrobbler(this, cfg);
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

			errorProvider.SetError(usernameTextBox, "Ingrese un nombre usuario correcto");
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
			notifyMsnMessegerCheckBox.Enabled = cfg.IsWindows;
			notifySystemTrayCheckBox.Checked = cfg.NotifySystemTray;
			startActivatedCheckBox.Checked = cfg.StartActivated;
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

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Open();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new AboutForm().ShowDialog();
		}

		private void openProfileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process.Start("http://www.last.fm/user/" + Program.Configuration.Username);
		}

	}
}