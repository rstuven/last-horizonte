using System;
using System.Web;

namespace LastHorizonte.Core
{
	public enum TrackStatus
	{
		Idle,
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

		public Track()
		{
			Status = TrackStatus.Idle;
		}

		public bool Equals(Track other)
		{
			return Artist == other.Artist && Title == other.Title && Status == other.Status;
		}

		public override string ToString()
		{
			return Artist + " - " + Title;
		}

		public string LastFmTitleUrl()
		{
			var artist = HttpUtility.UrlEncode(Artist);
			var title = HttpUtility.UrlEncode(Title);
			return String.Format("http://www.last.fm/music/{0}/_/{1}", artist, title);
		}

		public string LastFmArtistUrl()
		{
			var artist = HttpUtility.UrlEncode(Artist);
			return String.Format("http://www.last.fm/music/{0}", artist);
		}

		public Lastfm.Services.Track ToLastfmTrack(Lastfm.Services.Session session)
		{
			return new Lastfm.Services.Track(Artist, Title, session);
		}
	}
}