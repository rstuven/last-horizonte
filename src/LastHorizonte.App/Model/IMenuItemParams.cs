using System;
using System.Drawing;

namespace LastHorizonte
{
	internal interface IMenuItemParams
	{
		Image Image { get; set; }
		string Text { get; set; }
		OpeningHanlder OpeningHandler { get; set; }
	}

	internal delegate void OpeningHanlder(object sender, OpeningEventArgs args);

	internal class OpeningEventArgs : EventArgs
	{
		public IMenuItem MenuItem { get; set; }
	}

}