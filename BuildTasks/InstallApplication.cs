using System;
using System.Diagnostics;
using MFilesAPI;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MFiles.BuildTasks
{
	/// <summary>
	/// M-Files User Extensibility Framework - Application Installation Task.
	/// </summary>
	public class InstallApplication : Task
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public InstallApplication()
		{
			this.BuildEngine = base.BuildEngine;
			ServerAddress = "localhost";
			ProtocolSequence = "ncacn_ip_tcp";
			Port = "2266";
			AuthenticationType = MFAuthType.MFAuthTypeLoggedOnWindowsUser.ToString();
		}

		/// <summary>
		/// Vault GUID.
		/// </summary>
		[ Required ]
		public string VaultGuid { get; set; }

		#region Server Info

		/// <summary>
		/// Server Address / Endpoint
		/// </summary>
		[ Required ]
		public string ServerAddress { get; set; }

		/// <summary>
		/// Protocol sequence string.
		/// </summary>
		[ Required ]
		public string ProtocolSequence { get; set; }

		/// <summary>
		/// Port
		/// </summary>
		[ Required ]
		public string Port { get; set; }

		#endregion

		#region Login Info

		/// <summary>
		/// Authentication Type.
		/// </summary>
		[ Required ]
		public string AuthenticationType { get; set; }

		/// <summary>
		/// Username
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// Password
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Domain
		/// </summary>
		public string Domain { get; set; }

		#endregion

		#region Misc Info

		/// <summary>
		/// Local Computer Name.
		/// </summary>
		public string LocalComputerName { get; set; }

		/// <summary>
		/// Allow Anonymous Connection Flag
		/// </summary>
		public bool AllowAnonymousConnection { get; set; }

		#endregion

		#region App Install Info

		/// <summary>
		/// Application GUID.
		/// </summary>
		[ Required ]
		public string AppGUID { get; set; }

		/// <summary>
		/// Path to ( app.mfappx ).
		/// </summary>
		[ Required ]
		public string AppPath { get; set; }

		/// <summary>
		/// Should any existing applications be un-installed first?
		/// </summary>
		[ Required ]
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
					( MFAuthType ) Enum.Parse( typeof( MFAuthType ), AuthenticationType ),
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
			Vault vault = serverApp.LogInToVault( VaultGuid );

			bool result;
			try
			{
				// Determine if we should un-install the existing app.
				if( UninstallExisting )
					vault.CustomApplicationManagementOperations.UninstallCustomApplication( AppGUID );

			}
			catch( Exception ex )
			{
				// Log any errors.
				Debug.WriteLine( ex.Message );
			}

			// Try to install the app
			try
			{
				// Install from the AppPath.
				vault.CustomApplicationManagementOperations.InstallCustomApplication( AppPath );

				// Cycle the vault online / offline.
				serverApp.VaultManagementOperations.TakeVaultOffline( VaultGuid, true );
				serverApp.VaultManagementOperations.BringVaultOnline( VaultGuid );

				// Set a successful result.
				result = true;
			}
			catch( Exception ex )
			{
				// Log the error message.
				Debug.WriteLine( ex.Message );

				// Already exists is a success / user fail.
				if( ex.Message.StartsWith( "Already exists" ) )
				{
					// Set a successful result.
					result = true;
				}
				else
				{
					// Set a un-successful result.
					result = false;
				}
			}

			// Log Out
			if( vault.LoggedIn )
				vault.LogOutSilent();

			// Return the result.
			return result;
		}
	}
}