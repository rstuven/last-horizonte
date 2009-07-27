﻿using System;
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
			var track = Program.HorizonteScrobbler.LastPlayedTrack;
			if (track == null)
			{
				trackToolStripMenuItem.Visible = false;
			}
			else
			{
				var status = (track.Status == TrackStatus.Played ? "Sonó" : "Sonando");
				trackToolStripMenuItem.Text = status + ": " + track.ToString();
				trackToolStripMenuItem.Visible = true;
			}
			// Save in Tag property, as track can change in the meantime... 
			trackToolStripMenuItem.Tag = track;
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
			Process.Start("http://www.lastfm.es/join");
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

		private void trackLoveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var track = (Track)trackToolStripMenuItem.Tag;
			Program.HorizonteScrobbler.Love(track);
		}

		private void trackBanToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var track = (Track)trackToolStripMenuItem.Tag;
			Program.HorizonteScrobbler.Ban(track);
		}

		private void trackPageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var track = (Track)trackToolStripMenuItem.Tag;
			Process.Start(track.LastFmUrl());
		}

	}
}