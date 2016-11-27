using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MFiles.BuildTasks
{
	/// <summary>
	/// User Extensibility Framework : Directory Packaging Task.  Zips and Renames
	/// 
	/// <UsingTask TaskName="MFiles.BuildTasks.UXPack" AssemblyFile="path\to\MFiles.BuildTasks.dll"/>
	/// <Target Name="UXPackaging">
	///   <UXPack OutputName="MyAppName.mfappx" DirectoryToZip="C:\Git\MyApp\app_root\" />
	/// </Target>
	/// </summary>
	public class UXPack : Task
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public UXPack()
		{
			this.BuildEngine = base.BuildEngine;
		}

		/// <summary>
		/// Zip File Name ( likely MyAppName.mfappx )
		/// </summary>
		[Required]
		public string OutputName { get; set; }
		
		/// <summary>
		/// Path to the Directory Containing the UX-App.
		/// </summary>
		[Required]
		public string DirectoryToZip { get; set; }

		/// <summary>
		/// Packaging Logic
		/// </summary>
		/// <returns></returns>
		public override bool Execute()
		{
			// Build the full path.
			string fullpath = Path.Combine(Directory.GetParent(DirectoryToZip).FullName, OutputName);

			// Update the output window.
			Debug.WriteLine($"Packaging started: {fullpath}");

			// Determine if the file exists / deleting it when found.
			if( File.Exists( fullpath ) )
			{
				Debug.WriteLine($"File found.");
				File.Delete( fullpath );
				Debug.WriteLine($"File destroyed.");
			}
			else
			{
				Debug.WriteLine($"File not found.");
			}

			// Double check on the file destruction.
			if ( File.Exists( fullpath ) )
			{
				Debug.WriteLine( $"Failed to destroy existing file ( {fullpath} )" );
				return false;
			}

			// Zip the directory up as an application.
			ZipDirectory(DirectoryToZip, fullpath);

			// Log the success.
			Debug.WriteLine("Application package generated:\n");
			Debug.WriteLine( fullpath );

			// Return a success.
			return true;
		}

		/// <summary>
		/// Zips a Directory
		/// </summary>
		/// <param name="targetDir">Directory to be Zipped</param>
		/// <param name="zipFile">Output Filename</param>
		public static void ZipDirectory(string targetDir, string zipFile)
		{
			// Zip, quickly.
			ZipFile.CreateFromDirectory(targetDir, zipFile, CompressionLevel.Fastest, false);
		}
	}
}