using System.Windows.Forms;

namespace LastHorizonte
{
	internal class WinFormsMenuItemHanlder : IMenuItem
	{
		private readonly ToolStripMenuItem item;

		public WinFormsMenuItemHanlder(ToolStripMenuItem item)
		{
			this.item = item;
		}

		#region Implementation of IMenuItem

		public bool Checked
		{
			get { return item.Checked; }
			set { item.Checked = value; }
		}

		public bool Enabled
		{
			get { return item.Enabled; }
			set { item.Enabled = value; }
		}

		public bool Visible
		{
			get { return item.Visible; }
			set { item.Visible = value; }
		}

		public string Text
		{
			get { return item.Text; }
			set { item.Text = value; }
		}

		#endregion
	}
}