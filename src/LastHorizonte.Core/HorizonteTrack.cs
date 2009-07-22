using System;

namespace LastHorizonte.Core
{
	public class HorizonteTrack : IEquatable<HorizonteTrack>
	{
		public string Artist { get; set; }
		public string Title { get; set; }
		public string Status { get; set; }

		public bool Equals(HorizonteTrack other)
		{
			return Artist == other.Artist && Title == other.Title && Status == other.Status;
		}

		public bool IsPlaying
		{
			get { return Status == "En Horizonte"; }
		}

		public bool IsError
		{
			get { return Status == "Error"; }
		}

		public bool IsEmpty
		{
			get { return String.IsNullOrEmpty(Title); }
		}

		public override string ToString()
		{
			return Artist + " - " + Title;
		}
	}
}