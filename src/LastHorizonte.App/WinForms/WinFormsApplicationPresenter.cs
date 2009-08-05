using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Growl.DisplayStyle;
using LastHorizonte.Core;
using LastHorizonte.WinForms;

namespace LastHorizonte
{
	internal class WinFormsApplicationPresenter : IApplicationPresenter
	{
		private static NotifyIcon notifyIcon;
		private OptionsForm optionsForm;

		public void Initialize()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(true);
			CreateOptionsForm();
		}

		private void CreateOptionsForm()
		{
			this.optionsForm = new OptionsForm();
			this.optionsForm.CreateControl();
			this.optionsForm.StartPosition = FormStartPosition.Manual;
			this.optionsForm.Location = new Point(-5000, -5000);
			// Must be shown first just to get a window handle used by BeginInvoke (see notifications).
			this.optionsForm.Show();
			this.optionsForm.Hide();
			var screen = Screen.FromControl(this.optionsForm);
			this.optionsForm.Location = new Point((screen.Bounds.Width-this.optionsForm.Width)/2,(screen.Bounds.Height-this.optionsForm.Height)/2);
		}

		public void Start(RadioScrobbler radioScrobbler)
		{
			Application.ApplicationExit += ((sender, e) =>
			{
				if (notifyIcon != null)
				{
					notifyIcon.Visible = false;
				}
				radioScrobbler.Stop();
			});

			Application.Run();
		}

		public void CreateNotifyIcon(IMenuItemParams[] items, string text)
		{
			notifyIcon = new NotifyIcon
			{
				Icon = optionsForm.Icon,
				Visible = true,
				ContextMenuStrip = CreateContextMenu(items),
				Text = text
			};
			ShowBalloonTipInfo(Application.ProductName, notifyIcon.Text);

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

		public void ShowBalloonTipTrack(string status, Track track)
		{
			var notification = new Notification
			{
				Title = status,
				Description = track.ToString(),
				UUID = Guid.NewGuid().ToString(),
				NotificationID = Guid.NewGuid().ToString(),
				ApplicationName = Application.ProductName,
			};

			this.optionsForm.BeginInvoke(new Action<Notification>(n =>
		{
				var window = GetNotificationWindow(n);
				window.SetTrack(track);
				window.Show();
			}), notification);

			//notifyIcon.ShowBalloonTip(0, title, text, ToolTipIcon.Info);
		}

		public void ShowBalloonTipInfo(string title, string text)
		{
			var notification = new Notification
			{
				Title = title,
				Description = text,
				UUID = Guid.NewGuid().ToString(),
				NotificationID = Guid.NewGuid().ToString(),
				ApplicationName = Application.ProductName,
			};

			this.optionsForm.BeginInvoke(new Action<Notification>(n =>
			{
				var window = GetNotificationWindow(n);
				window.Show();
			}), notification);

			//notifyIcon.ShowBalloonTip(0, title, text, ToolTipIcon.Info);
			}

		private MyNotificationWindow GetNotificationWindow(Notification n)
		{
			var window = new MyNotificationWindow();
			window.SetNotification(n);
			window.AfterLoad += ((sender, e) => 
			{
				notificationLayoutManager.Add(window);
			});
			window.FormClosed += ((sender1, e1) => 
			{
				notificationLayoutManager.Remove(window);
			});
			return window;
		}

		LayoutManager notificationLayoutManager = new LayoutManager(LayoutManager.AutoPositionDirection.UpLeft, 10, 10);

		public void ShowBalloonTipError(string title, string text)
		{
			notifyIcon.ShowBalloonTip(0, title, text, ToolTipIcon.Error);
		}

		public void SetNotifyIconText(string format, params object[] args)
		{
			var text = String.Format(format, args);
			// Maxmimun length supported by NotifyIcon.Text is 63 characters.
			if (text.Length > 63)
			{
				text = text.Substring(0, 60) + "...";
			}
			notifyIcon.Text = text;
		}

		public void OpenAuthentication()
		{
			optionsForm.OpenWithAuthenticationError();
		}

		private static ContextMenuStrip CreateContextMenu(IMenuItemParams[] items)
		{
			var contextMenuStrip = new ContextMenuStrip();
			var list = GetList(items, null);
			contextMenuStrip.Items.AddRange(list);

			contextMenuStrip.Opening += ((sender, e) =>
			{
				foreach (ToolStripItem item in contextMenuStrip.Items)
				{
					var x = item.Tag as IMenuItemParams;
					if (x != null && x.OpeningHandler != null)
					{
						x.OpeningHandler(x, new OpeningEventArgs {MenuItem = new WinFormsMenuItemHanlder((ToolStripMenuItem)item)});
					}
				}
			});

			return contextMenuStrip;
		}

		public void Exit()
		{
			Application.Exit();
		}

		public void OpenAboutForm()
		{
			using (var form = new AboutForm())
			{
				form.ShowDialog();
			}
		}

		public void OpenOptionsForm()
		{
			optionsForm.Open();
		}


		private static ToolStripItem[] GetList(IEnumerable<IMenuItemParams> items, ImageMenuItemParams parent)
		{
			var list = new List<ToolStripItem>();
			foreach (var item in items)
			{
				if (item.Text == "-")
				{
					list.Add(new ToolStripSeparator());
					continue;
				}
				var menuitem = item as ImageMenuItemParams;
				if (menuitem != null)
				{
					menuitem.Parent = parent;
					var menuItem = GetMenuItem(menuitem);
					if (menuitem.Items != null)
					{
						menuItem.DropDownItems.AddRange(GetList(menuitem.Items, menuitem));
					}
					menuItem.Tag = item;
					list.Add(menuItem);
					continue;
				}
				var checkedmenuitem = item as CheckedMenuItemParams;
				if (checkedmenuitem != null)
				{
					var menuItem = GetCheckedMenuItem(checkedmenuitem);
					menuItem.Tag = item;
					list.Add(menuItem);
					continue;
				}
			}
			return list.ToArray();
		}

		private static ToolStripMenuItem GetCheckedMenuItem(CheckedMenuItemParams @params)
		{
			var menuItem = GetMenuItem(new ImageMenuItemParams
			{
				Text = @params.Text
			});
			menuItem.CheckOnClick = true;
			if (@params.Handler != null)
			{
				menuItem.Click +=
					((sender, e) =>
					 @params.Handler(@params, new CheckedMenuItemEventArgs {Checked = ((ToolStripMenuItem) sender).Checked})
					);
			}
			return menuItem;
		}


		private static ToolStripMenuItem GetMenuItem(ImageMenuItemParams @params)
		{
			var menuItem = new ToolStripMenuItem
			{
				Image = @params.Image,
				Text = @params.Text
			};
			if (@params.Handler != null)
			{
				menuItem.Click +=((sender, e)=>
				{
					@params.Handler(@params, new EventArgs());
				});
			}
			return menuItem;
		}
	}
}
