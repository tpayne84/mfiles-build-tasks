using System;
using MFilesAPI;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MFiles.BuildTasks
{
	/// <summary>
	/// VAF Module Packaging Task.
	/// </summary>
	public class InstallApplication : Task
	{
		public InstallApplication()
		{
			this.BuildEngine = base.BuildEngine;
			ServerAddress = "localhost";
			ProtocolSequence = "ncacn_ip_tcp";
			Port = "2266";
			AuthenticationType = MFAuthType.MFAuthTypeLoggedOnWindowsUser.ToString();
		}

		[Required]
		public string VaultGuid { get; set; }

		#region Server Info

		[Required]
		public string ServerAddress { get; set; }

		[Required]
		public string ProtocolSequence { get; set; }

		[Required]
		public string Port { get; set; }

		#endregion

		#region Login Info

		[Required]
		public string AuthenticationType { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }

		public string Domain { get; set; }

		#endregion

		#region Misc Info

		public string LocalComputerName { get; set; }

		public bool AllowAnonymousConnection { get; set; }

		#endregion

		#region App Install Info

		[Required]
		public string AppPath { get; set; }

		[Required]
		public bool UninstallExisting { get; set; }

		#endregion
			
		/// <summary>
		/// Application Installation Logic
		/// </summary>
		/// <returns></returns>
		public override bool Execute()
		{
			// Connect to Vault
			MFilesServerApplication serverApp = new MFilesServerApplication();
			serverApp.Connect(
				( MFAuthType ) Enum.Parse(typeof(MFAuthType), AuthenticationType),
				Username,
				Password,
				Domain,
				ProtocolSequence,
				ServerAddress,
				Port,
				LocalComputerName,
				AllowAnonymousConnection
			);

			// Log into the Vault
			Vault vault = serverApp.LogInToVault(VaultGuid);

			bool result;

			// try to install the app
			try
			{
				vault.CustomApplicationManagementOperations.InstallCustomApplication(AppPath);
				result = true;
			}
			catch (Exception ex)
			{
				if( ex.Message.StartsWith("Already exists") )
				{
					result = true;
				}
				else
				{
					result = false;					
				}
			}

			// Log Out
			vault.LogOutSilent();

			return result;
		}
	}
}