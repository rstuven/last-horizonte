using System;
using System.Collections.Generic;
using System.IO;
using Gdk;
using Gtk;
using LastHorizonte.Core;
using LastHorizonte.Properties;
using GtkImage = Gtk.Image;
using GdiImage = System.Drawing.Image;

namespace LastHorizonte
{
	class GtkApplicationPresenter : IApplicationPresenter
	{
		private StatusIcon trayIcon;
		private OptionsForm optionsForm;

		#region Implementation of IApplicationController

		public void Initialize()
		{
			Application.Init();
			this.optionsForm = new OptionsForm();
		}

		public void Start(HorizonteScrobbler horizonteScrobbler)
		{
			GLib.Timeout.Add(100, delegate
			{
				System.Windows.Forms.Application.DoEvents();
				return true;
			});
			Application.Run();
		}

		public void CreateNotifyIcon(IMenuItemParams[] items, string text)
		{
			var appIcon = ToPixbuf(Resources.logo);
			this.trayIcon = new StatusIcon(appIcon)
			{
				Visible = true,
				Tooltip = text
			};
			trayIcon.Activate += delegate
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
			trayIcon.PopupMenu += (o, e) =>
			{
				var popupMenu = new Menu();

				var subitems = GetList(items, null);
				foreach (var subitem in subitems)
				{
					popupMenu.Add(subitem);
				}

				popupMenu.Show();
				popupMenu.Popup();
			};
		}

		public void ShowBalloonTipInfo(string title, string text)
		{
			//var notify = new Notification
			//{
			//    Summary = title, 
			//    Body = text, 
			//    Urgency = Urgency.Normal, 
			//    IconName = Stock.DialogInfo
			//};
			//notify.AttachToStatusIcon(this.trayIcon);
			////notify.AddAction("name", "label", delegate
			////{
			////});
			//notify.Show();
		}

		public void ShowBalloonTipError(string title, string text)
		{
			//var notify = new Notification
			//{
			//    Summary = title,
			//    Body = text,
			//    Urgency = Urgency.Normal,
			//    IconName = Stock.DialogInfo
			//};
			//notify.AttachToStatusIcon(this.trayIcon);
			////notify.AddAction("name", "label", delegate
			////{
			////});
			//notify.Show();
		}

		public void SetNotifyIconText(string format, params object[] args)
		{
			this.trayIcon.Tooltip = String.Format(format, args);
		}

		public void OpenAuthentication()
		{
			optionsForm.OpenWithAuthenticationError();
		}

		public void Exit()
		{
			trayIcon.Visible = false;
			Application.Quit();
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

		#endregion

		private static Widget[] GetList(IEnumerable<IMenuItemParams> items, MenuItemParams parent)
		{
			var list = new List<Widget>();
			foreach (var item in items)
			{
				if (item.Text == "-")
				{
					list.Add(new SeparatorMenuItem { Visible = true });
					continue;
				}
				var menuitem = item as MenuItemParams;
				if (menuitem != null)
				{
					menuitem.Parent = parent;
					var menuItem = GetMenuItem(menuitem);
					if (menuitem.Items != null)
					{
						var submenu = new Menu();
						var subitems = GetList(menuitem.Items, menuitem);
						foreach (var subitem in subitems)
						{
							submenu.Add(subitem);
						}
						menuItem.Submenu = submenu;
					}
					list.Add(menuItem);
					continue;
				}
				var checkedmenuitem = item as CheckedMenuItemParams;
				if (checkedmenuitem != null)
				{
					var menuItem = GetCheckedMenuItem(checkedmenuitem);
					list.Add(menuItem);
					continue;
				}
			}
			return list.ToArray();
		}

		private static ImageMenuItem GetMenuItem(MenuItemParams @params)
		{
			var menuItem = new ImageMenuItem(@params.Text)
			{
				Image = ToImage(@params.Image),
				Visible = true
			};
			if (@params.Handler != null)
			{
				menuItem.Activated += ((sender, e) =>
				{
					@params.Handler(@params, new EventArgs());
				});
			}
			if (@params.OpeningHandler != null)
			{
				@params.OpeningHandler(@params, new OpeningEventArgs { MenuItem = new GtkMenuItemHanlder(menuItem) });
			}
			return menuItem;
		}

		private static CheckMenuItem GetCheckedMenuItem(CheckedMenuItemParams @params)
		{
			var menuItem = new CheckMenuItem(@params.Text)
			{
				Visible = true
			};
			if (@params.Handler != null)
			{
				menuItem.Toggled += ((sender, e) =>
				{
					@params.Handler(@params, new CheckedMenuItemEventArgs { Checked = ((CheckMenuItem)sender).Active });
				});
			}
			if (@params.OpeningHandler != null)
			{
				@params.OpeningHandler(@params, new OpeningEventArgs { MenuItem = new GtkMenuItemHanlder(menuItem) });
			}
			return menuItem;
		}

		private static Pixbuf ToPixbuf(System.Drawing.Icon gdiImage)
		{
			if (gdiImage == null)
			{
				return null;
			}
			using (var stream = new MemoryStream())
			{
				gdiImage.Save(stream);
				stream.Position = 0;
				return new Pixbuf(stream);
			}
		}

		private static GtkImage ToImage(GdiImage gdiImage)
		{
			if (gdiImage == null)
			{
				return null;
			}
			using (var stream = new MemoryStream())
			{
				gdiImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
				stream.Position = 0;
				return new GtkImage(stream);
			}
		}
	}
}
