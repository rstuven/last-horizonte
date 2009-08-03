using System;

namespace LastHorizonte
{
	internal class CheckedMenuItemParams : IMenuItemParams
	{
		public string Text { get; set; }
		public CheckedMenuItemEventHandler Handler { get; set; }
		public OpeningHanlder OpeningHandler { get; set; }
	}

	internal delegate void CheckedMenuItemEventHandler(object sender, CheckedMenuItemEventArgs args);

	internal class CheckedMenuItemEventArgs : EventArgs
	{
		public bool Checked { get; set; }
	}
	
}
