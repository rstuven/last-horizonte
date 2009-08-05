using LastHorizonte.Core;

namespace LastHorizonte
{
	internal interface IApplicationPresenter
	{
		void Initialize();
		void Start(RadioScrobbler radioScrobbler);
		void CreateNotifyIcon(IMenuItemParams[] items, string text);
		void ShowBalloonTipTrack(string status, Track track);
		void ShowBalloonTipInfo(string title, string text);
		void ShowBalloonTipError(string title, string text);
		void SetNotifyIconText(string format, params object[] args);
		void OpenAuthentication();
		void Exit();
		void OpenAboutForm();
		void OpenOptionsForm();
	}
}