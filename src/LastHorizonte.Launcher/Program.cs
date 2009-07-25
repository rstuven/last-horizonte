using System;

namespace LastHorizonte.Launcher
{
	class Program
	{
		static void Main()
		{
			do
			{
				Environment.ExitCode = 0;
				var setup = new AppDomainSetup();
				setup.ShadowCopyFiles = "true";
				var domain = AppDomain.CreateDomain("", AppDomain.CurrentDomain.Evidence, setup);
				domain.ExecuteAssemblyByName("LastHorizonte.App", AppDomain.CurrentDomain.Evidence);
			} while (Environment.ExitCode == 2);
		}
	}
}
