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
							Program.HorizonteScrobbler.Stop();
						}
					},
					OpeningHandler = (semder, e) =>
					{
						e.MenuItem.Checked = Program.HorizonteScrobbler.IsStarted;
					}
				},
				new MenuItemParams
				{
					Image = Resources.music,
					Text = "Tema",
					OpeningHandler = (sender, e)=>
					{
						var track = Program.HorizonteScrobbler.LastPlayedTrack;
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
						((MenuItemParams) sender).Tag = track;
					},
					Items = new[]
					{
						new MenuItemParams
						{
							Image = Resources.love,
							Text = "Favorito",
							Handler = (sender, e) =>
							{
								var track = (Track) ((MenuItemParams) sender).Parent.Tag;
								Program.HorizonteScrobbler.Love(track);
							}
						},
						new MenuItemParams
						{
							Image = Resources.ban,
							Text = "Vetar",
							Handler = (sender, e) =>
							{
								var track = (Track) ((MenuItemParams) sender).Parent.Tag;
								Program.HorizonteScrobbler.Ban(track);
							}
						},
						new MenuItemParams
						{
							Text = "Abrir página en Last.fm",
							Handler = (sender, e) =>
							{
								var track = (Track) ((MenuItemParams) sender).Parent.Tag;
								Process.Start(track.LastFmUrl());
							}
						}
					}
				},
				new MenuItemParams
				{
					Image = Resources.profile,
					Text = "Abrir mi perfil en Last.fm",
					Handler = (sender, e) =>
					{
						Process.Start("http://www.last.fm/user/" + Program.Configuration.Username);
					},
					OpeningHandler = (sender, e)=>
					{
						e.MenuItem.Enabled = Program.HorizonteScrobbler.IsInitialized;
					}
				},
				new MenuItemParams
				{
					Image = Resources.options,
					Text = "Opciones...",
					Handler = (sender, e) =>
					{
						application.OpenOptionsForm();
					}
				},
				new MenuItemParams
				{
					Image = Resources.information,
					Text = "Acerca de...",
					Handler = (sender, e) =>
					{
						application.OpenAboutForm();
					}
				},
				new MenuItemParams
				{
					Text = "-",
				},
				new MenuItemParams
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
