namespace LastHorizonte
{
	internal interface IMenuItem
	{
		bool Checked { get; set; }
		bool Enabled { get; set; }
		bool Visible { get; set; }
		string Text { get; set; }
	}
}