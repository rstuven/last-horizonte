using System;
using System.Drawing;

namespace LastHorizonte
{
	internal class ImageMenuItemParams : IMenuItemParams
	{
		public Image Image { get; set; }
		public string Text { get; set; }
		public EventHandler Handler { get; set; }
		public IMenuItemParams[] Items { get; set; }
		public ImageMenuItemParams Parent { get; set; }
		public object Tag { get; set; }
		public OpeningHanlder OpeningHandler { get; set; }
	}
}