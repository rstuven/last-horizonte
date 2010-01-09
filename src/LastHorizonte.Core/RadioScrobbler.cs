using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using Lastfm.Scrobbling;
using Lastfm.Services;

namespace LastHorizonte.Core
{
	public class RadioScrobbler
	{
		private Configuration configuration;
		private ScrobbleManager scrobbleManager;
		private Session session;
		private string feedUrl;
		private readonly BackgroundWorker worker;
		private Track lastPlayedTrack;

		public bool IsStarted
		{
			get { return worker.IsBusy; }
		}

		public bool IsInitialized
		{
			get { return configuration != null; }
		}

		public Track LastPlayedTrack
		{
			get { return lastPlayedTrack; }
		}

		public RadioScrobbler()
		{
			this.worker = new BackgroundWorker();
			worker.DoWork += worker_DoWork;
			worker.WorkerSupportsCancellation = true;
		}

		public void Initialize(Configuration configuration)
		{
			Trace.WriteLine("Initializing...");
			try
			{
				session = GetSession(configuration);
				scrobbleManager = GetScrobbleManager(configuration, session);
			}
			catch (Exception ex)
			{
				var lastfmServiceException = ex as ServiceException;
				if (lastfmServiceException != null)
				{
					if (lastfmServiceException.Type == ServiceExceptionType.AuthenticationFailed)
					{
						// Wrap exception in AuthenticationException class.
						throw new AuthenticationException(ex.Message, ex);
					}
				}
				throw;
			}
			feedUrl = GetFeedUrl();
			this.configuration = configuration;
			Trace.WriteLine("Initialized");
		}

		public void Start(Configuration configuration)
		{
			if (IsInitialized)
			{
				Start();				
			}
			else
			{
				Initialize(configuration);
				if (configuration.StartActivated)
				{
					Start();
				}
			}
		}
		public void Start()
		{
			if (!worker.IsBusy)
			{
				worker.RunWorkerAsync();
			}
		}

		public void Stop()
		{
			if (worker.IsBusy)
			{
				worker.CancelAsync();
			}
			NotifyMsnMessenger(null);
		}

		void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			InvokeStarted(null);
			Trace.WriteLine("Started");

			Track lastPlayingTrack = null;
			Track lastScrobbledTrack = null;
			Track lastTrack = null;
			var lastPlayedTrackStarted = DateTime.Now;
			var cycles = 0;

			while (!worker.CancellationPending)
			{
				if (cycles == 0)
				{
					cycles = 15; // 15 seconds approx.
					var track = GetCurrentTrack(feedUrl);
					var lastPlayedTrackHasChanged = (lastPlayingTrack != null && !lastPlayingTrack.Equals(track));
					var isFirstTime = (lastTrack == null || !lastTrack.Equals(track));

					if (lastPlayedTrackHasChanged)
					{
						var newScrobbledTrack = (lastScrobbledTrack == null || !lastScrobbledTrack.Equals(lastPlayingTrack));
						var duration = DateTime.Now - lastPlayedTrackStarted;
						if (newScrobbledTrack && duration > new TimeSpan(0, 0, 0, 30))
						{
							var entry = new Entry(
								lastPlayingTrack.Artist,
								lastPlayingTrack.Title,
								lastPlayedTrackStarted,
								PlaybackSource.NonPersonalizedBroadcast,
								duration,
								ScrobbleMode.Played
								);
							scrobbleManager.Queue(entry);
							lastScrobbledTrack = lastPlayingTrack;
							InvokeScrobbled(new TrackEventArgs() {Track = lastPlayingTrack});
							Trace.WriteLine(String.Format("Scrobbled: {0} ({1})", lastPlayingTrack, duration));
						}
						lastPlayedTrackStarted = DateTime.Now;
					}
					if (track.Status == TrackStatus.Playing)
					{
						var duration = DateTime.Now - lastPlayedTrackStarted;
						NotifyMsnMessenger(track);
						if (configuration.NotifyLastFm)
						{
							var nowplayingTrack = new NowplayingTrack(track.Artist, track.Title, duration);
							scrobbleManager.ReportNowplaying(nowplayingTrack);
						}
						Trace.WriteLine(String.Format("Now playing: {0} ({1})", track, duration));
						lastPlayingTrack = track;
						lastPlayedTrack = track;
					}
					else
					{
						NotifyMsnMessenger(null);
						lastPlayingTrack = null;
						if (lastPlayedTrack.Status == TrackStatus.Playing)
						{
							lastPlayedTrack.Status = TrackStatus.Played;
						}
						Trace.WriteLine(String.Format("{0}: {1}", track.Status, track));
					}
					InvokePlaying(new PlayingEventArgs
					{
						Track = track,
						IsFirstTime = isFirstTime
					});
					lastTrack = track;
				}
				cycles--;
				Thread.Sleep(1000);
			}
			InvokeStopped(null);
			Trace.WriteLine("Stopped");
		}

		private void NotifyMsnMessenger(Track track)
		{
			if (configuration != null && Configuration.IsRunningOnWindows)
			{
				if (configuration.NotifyMsnMessenger)
				{
					MsnMessenger.UpdateMusic(track);
				}
				else
				{
					MsnMessenger.UpdateMusic(null);
				}
			}
		}



		private static Session GetSession(Configuration configuration)
		{
			var username = configuration.Username;
			var password = configuration.Password;

			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
			{
				throw new AuthenticationException();
			}

			const string API_KEY = "fe136e7517c55e3d9f7aa295c2b02a72";
			const string API_SECRET = "433cdcd4e96914dbcd71f176654e3bb3";

			var session = new Session(API_KEY, API_SECRET);
			session.Authenticate(username, password);

			return session;
		}

		private static ScrobbleManager GetScrobbleManager(Configuration configuration, Session session)
		{
			var clientVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
			var connection = new Connection("hzt", clientVersion, configuration.Username, session);
			return new ScrobbleManager(connection);
		}

		private static Track GetCurrentTrack(string feedUrl)
		{
			try
			{
				var request = WebRequest.Create(feedUrl);
				request.Timeout = 10000;
				using (var response = request.GetResponse())
				{
					using (var reader = new StreamReader(response.GetResponseStream()))
					{
						var feed = reader.ReadToEnd();
						feed = CleanInvalidXml(feed);

						var xmlDocument = new XmlDocument();
						xmlDocument.LoadXml(feed);
						var track = xmlDocument.SelectSingleNode("//cancion");
						return new Track
						{
							Artist = GetTrackProperty(track, "grupo"),
							Title = GetTrackProperty(track, "titulo"),
							Status = GetTrackStatus(track, "estado")
						};
					}
				}
			}
			catch (Exception ex)
			{
				return new Track
				{
					Status = TrackStatus.Error,
					Title = ex.Message
				};
			}
		}

		private static readonly Regex CleanInvalidXmlRegex = new Regex(@"<!--(.*)-->[\r\n]?", RegexOptions.Multiline | RegexOptions.Compiled);

		private static string CleanInvalidXml(string xml)
		{
			xml = CleanInvalidXmlRegex.Replace(xml, "");
			xml = xml
				.Replace("&", "&amp;")
				.Replace("´", "'")
				;
			return xml;
		}

		private static string GetTrackProperty(XmlNode node, string name)
		{
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(node.SelectSingleNode(name).InnerText.Trim().ToLower());
		}

		private static TrackStatus GetTrackStatus(XmlNode node, string name)
		{
			var status = node.SelectSingleNode(name).InnerText.Trim().ToLower();
			if (status == "en horizonte")
			{
				return TrackStatus.Playing;
			}
			if (status == "luego en horizonte")
			{
				return TrackStatus.Coming;
			}
			return TrackStatus.Idle;
		}

		private static string GetFeedUrl()
		{
			var request = WebRequest.Create("http://www.horizonte.cl");
			request.Timeout = 10000;
			using (var response = request.GetResponse())
			{
				using (var reader = new StreamReader(response.GetResponseStream()))
				{
					var home = reader.ReadToEnd();
					return Regex.Match(home, "http://(.*)/hzsonando.xml").ToString();
				}
			}
		}

		public void Love(Track track)
		{
			track.ToLastfmTrack(session).Love();
			InvokeLoved(new TrackEventArgs { Track = track });
		}

		public void Ban(Track track)
		{
			track.ToLastfmTrack(session).Ban();
			InvokeBanned(new TrackEventArgs { Track = track });
		}


		#region Events

		public event EventHandler Started;
		private void InvokeStarted(EventArgs e)
		{
			var eventHandler = Started;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		public event EventHandler Stopped;
		private void InvokeStopped(EventArgs e)
		{
			var eventHandler = Stopped;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		public event PlayingEventHandler Playing;
		private void InvokeScrobbled(TrackEventArgs eventArgs)
		{
			var eventHandler = Scrobbled;
			if (eventHandler != null)
			{
				eventHandler(this, eventArgs);
			}
		}

		public event TrackEventHandler Scrobbled;
		private void InvokePlaying(PlayingEventArgs eventArgs)
		{
			var eventHandler = Playing;
			if (eventHandler != null)
			{
				eventHandler(this, eventArgs);
			}
		}

		public event TrackEventHandler Loved;
		private void InvokeLoved(TrackEventArgs eventArgs)
		{
			var eventHandler = Loved;
			if (eventHandler != null)
			{
				eventHandler(this, eventArgs);
			}
		}

		public event TrackEventHandler Banned;
		private void InvokeBanned(TrackEventArgs eventArgs)
		{
			var eventHandler = Banned;
			if (eventHandler != null)
			{
				eventHandler(this, eventArgs);
			}
		}

		#endregion
	}

	public delegate void TrackEventHandler(object sender, TrackEventArgs eventArgs);

	public class TrackEventArgs
	{
		public Track Track { get; set; }
	}

	public delegate void PlayingEventHandler(object sender, PlayingEventArgs eventArgs);

	public class PlayingEventArgs
	{
		public Track Track { get; set; }
		public bool IsFirstTime { get; set; }
	}
}