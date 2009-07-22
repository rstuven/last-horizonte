using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using Lastfm.Scrobbling;
using Lastfm.Services;

namespace LastHorizonte.Core
{
	public class HorizonteScrobbler
	{
		private Configuration configuration;
		private ScrobbleManager scrobbleManager;
		private string feedUrl;
		private readonly BackgroundWorker worker;

		public bool IsStarted
		{
			get { return worker.IsBusy; }
		}

		public bool IsInitialized
		{
			get { return configuration != null; }
		}

		public HorizonteScrobbler()
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
				scrobbleManager = GetScrobbleManager(configuration.Username, configuration.Password);
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

			HorizonteTrack lastPlayedTrack = null;
			HorizonteTrack lastScrobbledTrack = null;
			HorizonteTrack lastTrack = null;
			var lastPlayedTrackStarted = DateTime.Now;
			var cycles = 0;

			while (!worker.CancellationPending)
			{
				if (cycles == 0)
				{
					cycles = 15; // 15 seconds approx.
					var track = GetCurrentTrack(feedUrl);
					var lastPlayedTrackHasChanged = (lastPlayedTrack != null && !lastPlayedTrack.Equals(track));
					var isFirstTime = (lastTrack == null || !lastTrack.Equals(track));

					if (lastPlayedTrackHasChanged)
					{
						var newScrobbledTrack = (lastScrobbledTrack == null || !lastScrobbledTrack.Equals(lastPlayedTrack));
						var duration = DateTime.Now - lastPlayedTrackStarted;
						if (newScrobbledTrack && duration > new TimeSpan(0, 0, 0, 30))
						{
							var entry = new Entry(
								lastPlayedTrack.Artist,
								lastPlayedTrack.Title,
								lastPlayedTrackStarted,
								PlaybackSource.NonPersonalizedBroadcast,
								duration,
								ScrobbleMode.Played
								);
							scrobbleManager.Queue(entry);
							lastScrobbledTrack = lastPlayedTrack;
							InvokeScrobbled(new ScrobbledEventArgs() {Track = lastPlayedTrack});
							Trace.WriteLine(String.Format("Scrobbled: {0} ({1})", lastPlayedTrack, duration));
						}
						lastPlayedTrackStarted = DateTime.Now;
					}
					if (track.IsPlaying)
					{
						var duration = DateTime.Now - lastPlayedTrackStarted;
						NotifyMsnMessenger(track);
						if (configuration.NotifyLastFm)
						{
							var nowplayingTrack = new NowplayingTrack(track.Artist, track.Title, duration);
							scrobbleManager.ReportNowplaying(nowplayingTrack);
						}
						Trace.WriteLine(String.Format("Now playing: {0} ({1})", track, duration));
						lastPlayedTrack = track;
					}
					else
					{
						NotifyMsnMessenger(null);
						lastPlayedTrack = null;
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

		private void NotifyMsnMessenger(HorizonteTrack track)
		{
			if (configuration != null && configuration.IsWindows)
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


		private static ScrobbleManager GetScrobbleManager(string username, string password)
		{
			const string API_KEY = "fe136e7517c55e3d9f7aa295c2b02a72";
			const string API_SECRET = "433cdcd4e96914dbcd71f176654e3bb3";

			var session = new Session(API_KEY, API_SECRET);
			session.Authenticate(username, password);

			var connection = new Connection("hzt", "1.0", username, session);
			return new ScrobbleManager(connection);
		}

		private static HorizonteTrack GetCurrentTrack(string feedUrl)
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
						return new HorizonteTrack
						{
							Artist = GetTrackProperty(track, "grupo"),
							Title = GetTrackProperty(track, "titulo"),
							Status = GetTrackProperty(track, "estado")
						};
					}
				}
			}
			catch (Exception ex)
			{
				return new HorizonteTrack
				{
					Status = "Error",
					Title = ex.Message
				};
			}
		}

		private static readonly Regex CleanInvalidXmlRegex = new Regex("<!--(.*)-->\r\n", RegexOptions.Multiline | RegexOptions.Compiled);

		private static string CleanInvalidXml(string xml)
		{
			xml = CleanInvalidXmlRegex.Replace(xml, "");
			xml = xml.Replace("&", "&amp;");
			return xml;
		}

		private static string GetTrackProperty(XmlNode node, string name)
		{
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(node.SelectSingleNode(name).InnerText.Trim().ToLower());
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
		private void InvokeScrobbled(ScrobbledEventArgs eventArgs)
		{
			var eventHandler = Scrobbled;
			if (eventHandler != null)
			{
				eventHandler(this, eventArgs);
			}
		}

		public event ScrobbledEventHandler Scrobbled;
		private void InvokePlaying(PlayingEventArgs eventArgs)
		{
			var eventHandler = Playing;
			if (eventHandler != null)
			{
				eventHandler(this, eventArgs);
			}
		}
		#endregion
	}

	public delegate void ScrobbledEventHandler(object sender, ScrobbledEventArgs eventArgs);

	public class ScrobbledEventArgs
	{
		public HorizonteTrack Track { get; set; }
	}

	public delegate void PlayingEventHandler(object sender, PlayingEventArgs eventArgs);

	public class PlayingEventArgs
	{
		public HorizonteTrack Track { get; set; }
		public bool IsFirstTime { get; set; }
	}
}