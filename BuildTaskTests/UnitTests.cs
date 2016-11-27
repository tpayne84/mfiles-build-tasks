using MFiles.BuildTasks;
using MFilesAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuildTaskTests
{
	[TestClass]
	public class UnitTests
	{
		[TestMethod]
		public void Test_UXPack()
		{
			var uxPack = new UXPack(); 
			uxPack .DirectoryToZip = @"C:\test";
			uxPack.OutputName = "ux_app.mfappx" ;
			uxPack.Execute();
		}

		[ TestMethod ]
		public void Test_InstallApplication()
		{
			var installApp = new InstallApplication();
			installApp.AllowAnonymousConnection = false;
			installApp.AppPath = @"C:\test\ux_app.mfappx";
			installApp.AuthenticationType = MFAuthType.MFAuthTypeLoggedOnWindowsUser.ToString();
			installApp.LocalComputerName = "MSBuild";
			installApp.VaultGuid = "{65E7D063-FA85-4063-A904-AB44FBAA4C9F}";
			installApp.UninstallExisting = true;

			installApp.Execute();
		}

	}
}
