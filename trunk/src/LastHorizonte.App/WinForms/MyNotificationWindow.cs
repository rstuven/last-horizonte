using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Growl.DisplayStyle;
using LastHorizonte.Core;
using LastHorizonte.Properties;

namespace LastHorizonte.WinForms
{
	partial class MyNotificationWindow : NotificationWindow
	{
		private readonly Timer displayTimer;

		public MyNotificationWindow()
        {
            InitializeComponent();

            this.BackColor = Color.White;
            this.ForeColor = Color.FromArgb(51, 51, 51);

			this.Load += ((sender, e) =>
			{
				var screen = Screen.FromControl(this);
				var x = screen.WorkingArea.Right - this.Size.Width - 5;
				var y = screen.WorkingArea.Bottom - this.Size.Height - 30;
				this.Location = new Point(x, y);
			});

            this.Animator = new FadeAnimator(this);

			this.displayTimer = new Timer();
			this.displayTimer.Tick += ((sender, e) =>
			{
				this.displayTimer.Stop();
				var args = new FormClosingEventArgs(CloseReason.None, false);
				this.OnAutoClosing(this, args);
				if (!args.Cancel)
				{
					Close();
				}
			});
			this.displayTimer.Interval = 4000;
			this.MouseMove += ResetAutoClose;
			this.descriptionLabel.MouseMove += ResetAutoClose;
			this.titleLabel.MouseMove += ResetAutoClose;

			this.MouseDown += CloseOnMouseDown;
			this.titleLabel.MouseDown += CloseOnMouseDown;
			this.pictureBox1.MouseDown += CloseOnMouseDown;
			this.descriptionLabel.MouseDown += CloseOnMouseDown;
		}

		void CloseOnMouseDown(object sender, MouseEventArgs e)
		{
			this.Close();
		}

		private void ResetAutoClose(object sender, EventArgs e)
		{
			this.Opacity = 1.0;
			this.displayTimer.Stop();	
			this.displayTimer.Start();
		}

		public override void Show()
		{
			var currentForegroundWindow = Win32.GetForegroundWindow();
			base.Show();
			Win32.SetWindowPos(Handle, -1, 0, 0, 0, 0, Win32.SWP_NOSIZE | Win32.SWP_NOMOVE);
			Win32.SetForegroundWindow(currentForegroundWindow);
			this.displayTimer.Start();
		}

		protected override void OnPaintBackground(PaintEventArgs e)
        {
            var rect = new Rectangle(0, 0, this.Width, this.Height);

            e.Graphics.Clear(this.ForeColor);

            rect.Inflate(-1, -1);
            var b2 = new SolidBrush(this.BackColor);
            using (b2)
            {
                e.Graphics.FillRectangle(b2, rect);
            }
        }

        public override void SetNotification(Notification n)
        {
            base.SetNotification(n);

            if (n.Duration > 0) this.AutoClose(n.Duration * 1000);

            // handle the image. if the image is not set, move the other controls over to compensate
            var image = Resources.logo.ToBitmap();
            if (image != null)
            {
                this.pictureBox1.Image = image;
				this.pictureBox1.Size = new Size(32, 32);
                this.pictureBox1.Visible = true;

                int offset = this.pictureBox1.Width + 6;
                this.titleLabel.Left = this.titleLabel.Left + offset;
                this.titleLabel.Width = this.titleLabel.Width - offset;
                this.descriptionLabel.Left = this.descriptionLabel.Left + offset;
                this.descriptionLabel.Width = this.descriptionLabel.Width - offset;
            }
            else
            {
                this.pictureBox1.Visible = false;
            }

            this.titleLabel.Text = n.Title;
            this.descriptionLabel.Text = n.Description.Replace("\n", "\r\n");
			this.descriptionLabel.Links.Clear();
            this.Sticky = n.Sticky;
        }

		public void SetTrack(Track track)
		{
			const string separator = " - ";
			this.descriptionLabel.Text = track.Artist + separator + track.Title;
			this.descriptionLabel.Links.Add(0, track.Artist.Length, track.LastFmArtistUrl());
			this.descriptionLabel.Links.Add(track.Artist.Length + separator.Length, track.Title.Length, track.LastFmTitleUrl());
			this.descriptionLabel.LinkClicked += ((sender, e) => Process.Start((string)e.Link.LinkData));
			this.descriptionLabel.MouseDown -= CloseOnMouseDown;
		}

        /// <summary>
        /// By handling the LabelHeightChanged events, you can make sure you notification window
        /// will expand properly to fit all of the text. In order to take advantage of this event,
        /// you must use ExpandingLabel class in place of normal Labels.
        /// </summary>
        private void titleLabel_LabelHeightChanged(ExpandingLabel.LabelHeightChangedEventArgs args)
        {
            this.pictureBox1.Top += args.HeightChange;
            this.descriptionLabel.Top += args.HeightChange;
        }

		/// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.pictureBox1 != null) this.pictureBox1.Dispose();
                if(components != null) components.Dispose();
				if ((this.displayTimer != null)) this.displayTimer.Dispose();
			}
			base.Dispose(disposing);
        }
	}
}