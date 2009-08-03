using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using LastHorizonte.Core;

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
			this.optionsForm = new OptionsForm();
		}

		public void Start(HorizonteScrobbler horizonteScrobbler)
		{
			Application.ApplicationExit += ((sender, e) =>
			{
				if (notifyIcon != null)
				{
					notifyIcon.Visible = false;
				}
				horizonteScrobbler.Stop();
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

		public void ShowBalloonTipInfo(string title, string text)
		{
			if (Configuration.IsRunningOnMono)
			{
				return;
			}
			notifyIcon.ShowBalloonTip(0, title, text, ToolTipIcon.Info);
		}

		public void ShowBalloonTipError(string title, string text)
		{
			if (Configuration.IsRunningOnMono)
			{
				return;
			}
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

		private ContextMenuStrip CreateContextMenu(IMenuItemParams[] items)
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
