using System;

namespace LastHorizonte.Launcher
{
	class Program
	{
		static void Main()
		{
			do
			{
				// Reset exit code.
				// In this app, exit code 2 means "Restart"
				Environment.ExitCode = 0;

				// Start app in a new domain that use shadow copy files.
				// Shadow copies allow autoupdater to overwrite assemblies
				// while the app is running.
				var setup = new AppDomainSetup();
				setup.ShadowCopyFiles = "true";
				var domain = AppDomain.CreateDomain("", AppDomain.CurrentDomain.Evidence, setup);
				domain.ExecuteAssemblyByName("LastHorizonte.App", AppDomain.CurrentDomain.Evidence);
			} while (Environment.ExitCode == 2);
		}
	}
}
