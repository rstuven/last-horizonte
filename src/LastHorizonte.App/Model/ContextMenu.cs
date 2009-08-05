using System.Diagnostics;
using LastHorizonte.Core;
using LastHorizonte.Properties;

namespace LastHorizonte
{
	internal class ContextMenu
	{
		public static IMenuItemParams[] GetItems(IApplicationPresenter application)
		{
			return new IMenuItemParams[]
			{
				new CheckedMenuItemParams
				{
					Text = "Scrobbling activado",
					Handler = (sender, e) =>
					{
						if (e.Checked)
						{
							Program.InitializeAndStartScrobbler(Program.Configuration);
						}
						else
						{
							Program.RadioScrobbler.Stop();
						}
					},
					OpeningHandler = (semder, e) =>
					{
						e.MenuItem.Checked = Program.RadioScrobbler.IsStarted;
					}
				},
				new ImageMenuItemParams
				{
					Image = Resources.music,
					Text = "Tema",
					OpeningHandler = (sender, e)=>
					{
						var track = Program.RadioScrobbler.LastPlayedTrack;
						if (track == null)
						{
							e.MenuItem.Visible = false;
						}
						else
						{
							var status = (track.Status == TrackStatus.Played ? "Sonó" : "Sonando");
							e.MenuItem.Text = status + ": " + track.ToString();
							e.MenuItem.Visible = true;
						}
						// Save in Tag property, as track can change in the meantime... 
						((ImageMenuItemParams) sender).Tag = track;
					},
					Items = new[]
					{
						new ImageMenuItemParams
						{
							Image = Resources.love,
							Text = "Favorito",
							Handler = (sender, e) =>
							{
								var track = (Track) ((ImageMenuItemParams) sender).Parent.Tag;
								Program.RadioScrobbler.Love(track);
							}
						},
						new ImageMenuItemParams
						{
							Image = Resources.ban,
							Text = "Vetar",
							Handler = (sender, e) =>
							{
								var track = (Track) ((ImageMenuItemParams) sender).Parent.Tag;
								Program.RadioScrobbler.Ban(track);
							}
						},
						new ImageMenuItemParams
						{
							Text = "Abrir página en Last.fm",
							Handler = (sender, e) =>
							{
								var track = (Track) ((ImageMenuItemParams) sender).Parent.Tag;
								Process.Start(track.LastFmTitleUrl());
							}
						}
					}
				},
				new ImageMenuItemParams
				{
					Image = Resources.profile,
					Text = "Abrir mi perfil en Last.fm",
					Handler = (sender, e) =>
					{
						Process.Start("http://www.last.fm/user/" + Program.Configuration.Username);
					},
					OpeningHandler = (sender, e)=>
					{
						e.MenuItem.Enabled = Program.RadioScrobbler.IsInitialized;
					}
				},
				new ImageMenuItemParams
				{
					Image = Resources.options,
					Text = "Opciones...",
					Handler = (sender, e) =>
					{
						application.OpenOptionsForm();
					}
				},
				new ImageMenuItemParams
				{
					Image = Resources.information,
					Text = "Acerca de...",
					Handler = (sender, e) =>
					{
						application.OpenAboutForm();
					}
				},
				new ImageMenuItemParams
				{
					Text = "-",
				},
				new ImageMenuItemParams
				{
					Text = "Cerrar",
					Handler = (sender, e) =>
					{
						application.Exit();
					}
				}
			};
		}
	}

}
