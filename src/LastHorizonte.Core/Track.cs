using System;

namespace LastHorizonte.Core
{
	public enum TrackStatus
	{
		None,
		Playing,
		Coming,
		Played,
		Error,
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

		public string LastFmUrl()
		{
			var artist = Artist.Replace(" ", "+");
			var title = Title.Replace(" ", "+");
			return String.Format("http://www.last.fm/music/{0}/_/{1}", artist, title);
		}

		public Lastfm.Services.Track ToLastfmTrack(Lastfm.Services.Session session)
		{
			return new Lastfm.Services.Track(Artist, Title, session);
		}
	}
}