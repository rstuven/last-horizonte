using Gtk;

namespace LastHorizonte
{
	public class GtkMenuItemHanlder : IMenuItem
	{
		private readonly MenuItem item;

		public GtkMenuItemHanlder(MenuItem item)
		{
			this.item = item;
		}

		#region Implementation of IMenuItem

		public bool Checked
		{
			get { return ((CheckMenuItem) item).Active; }
			set { ((CheckMenuItem)item).Active = value; }
		}

		public bool Enabled
		{
			get { return item.Sensitive; }
			set { item.Sensitive = value; }
		}

		public bool Visible
		{
			get { return item.Child.Visible; }
			set { item.Visible = value; }
		}

		public string Text
		{
			get { return ((Label)item.Child).Text; }
			set { ((Label)item.Child).Text = value; }
		}

		#endregion
	}
}