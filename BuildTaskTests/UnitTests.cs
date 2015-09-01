using MFiles.BuildTasks;
using MFilesAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuildTaskTests
{
	[TestClass]
	public class UnitTests
	{
		[TestMethod]
		public void Test_ModulePack()
		{
			var modulePack = new ModulePack();
			modulePack.PathToBuilderConfiguration = @"C:\Program Files\M-Files\M-Files Module Packager\config.json";
			modulePack.PathToSolutionFile = @"C:\Users\travis.payne\Desktop\GitHub\SearchStringDeserializer\SearchStringDeserializer.sln";
			modulePack.Execute();
		}

		[TestMethod]
		public void Test_UXPack()
		{
			var uxPack = new UXPack(); 
			uxPack .DirectoryToZip = @"C:\Users\travis.payne\Desktop\GitHub\SearchStringDeserializer\SearchStringDeserializer\ux_app";
			uxPack.OutputName = "ux_app.mfappx" ;
			uxPack.Execute();
		}

		[ TestMethod ]
		public void Test_InstallApplication()
		{
			var installApp = new InstallApplication();
			installApp.AllowAnonymousConnection = false;
			installApp.AppPath = @"C:\Users\travis.payne\Desktop\GitHub\services-vault\App Packaging\ServicesVault.mfappx";
			installApp.AuthenticationType = MFAuthType.MFAuthTypeLoggedOnWindowsUser.ToString();
			installApp.LocalComputerName = "MSBuild";
			installApp.VaultGuid = "{33D0C40C-F023-41F9-A3CE-1FB732DA5554}";
			installApp.UninstallExisting = true;

			installApp.Execute();			
		}

	}
}
