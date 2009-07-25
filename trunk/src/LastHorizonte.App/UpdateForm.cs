using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Conversive.AutoUpdater;

namespace LastHorizonte
{
	public partial class UpdateForm : Form
	{
		private readonly AutoUpdater autoUpdater;
		private string latestChangesText;

		public UpdateForm(AutoUpdater autoUpdater)
		{
			this.autoUpdater = autoUpdater;
			InitializeComponent();
		}


		private void yesButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Yes;
			this.Close();
		}

		private void noButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.No;
			this.Close();
		}

		private void UpdateForm_Load(object sender, EventArgs e)
		{
			latestChangesText = 
				String.Format(
@"Tu versión: {0}
Nueva versión:{1}

{2}", 
				this.autoUpdater.CurrentAppVersion,
				this.autoUpdater.LatestConfigVersion,
				this.autoUpdater.LatestConfigChanges.Replace("\\n", "\r\n"));
			this.Text = Application.ProductName;
			this.latestChangesTextBox.Text = latestChangesText;
			var changeLogUrl = this.autoUpdater.AutoUpdateConfig.ChangeLogUrl;
			this.linkLabel.Links.Add(0, this.linkLabel.Text.Length, changeLogUrl);
			this.linkLabel.LinkClicked += ((sndr, ev) =>
			{
				Process.Start((string)ev.Link.LinkData);
			});

			KeepActivated();
		}

		private void KeepActivated()
		{
			new Thread(delegate()
			{
				while (this.Visible)
				{
					this.BeginInvoke(new Action(this.Activate));
					Thread.Sleep(1000);
				}
			}).Start();
		}

		delegate void Action();

		private void latestChangesTextBox_TextChanged(object sender, EventArgs e)
		{
			this.latestChangesTextBox.Text = latestChangesText;
		}
	}
}
