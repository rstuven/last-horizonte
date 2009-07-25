using System;

namespace LastHorizonte.Core
{
	public enum TrackStatus
	{
		None,
		Playing,
		Coming,
		Error
	}

	public class Track : IEquatable<Track>
	{
		public string Artist { get; set; }
		public string Title { get; set; }
		public TrackStatus Status { get; set; }

		public bool Equals(Track other)
		{
			return Artist == other.Artist && Title == other.Title && Status == other.Status;
		}

		public override string ToString()
		{
			return Artist + " - " + Title;
		}
	}
}