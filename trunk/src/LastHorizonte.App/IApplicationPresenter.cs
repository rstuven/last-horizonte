using LastHorizonte.Core;

namespace LastHorizonte
{
	internal interface IApplicationPresenter
	{
		void Initialize();
		void Start(HorizonteScrobbler horizonteScrobbler);
		void CreateNotifyIcon(IMenuItemParams[] items, string text);
		void ShowBalloonTipInfo(string title, string text);
		void ShowBalloonTipError(string title, string text);
		void SetNotifyIconText(string format, params object[] args);
		void OpenAuthentication();
		void Exit();
		void OpenAboutForm();
		void OpenOptionsForm();
	}
}