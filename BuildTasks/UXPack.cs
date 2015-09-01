using System.IO;
using System.IO.Compression;
using System.Linq;
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
			string fullpath = Path.Combine(Directory.GetParent(DirectoryToZip).FullName, OutputName);
			if( File.Exists( fullpath ) )
				File.Delete(fullpath);

			if( File.Exists( fullpath ) )
				return false;

			ZipDirectory(DirectoryToZip, fullpath);

			return true;
		}

		/// <summary>
		/// Zips a Directory
		/// </summary>
		/// <param name="targetDir">Directory to be Zipped</param>
		/// <param name="zipFile">Output Filename</param>
		public static void ZipDirectory(string targetDir, string zipFile)
		{
			ZipFile.CreateFromDirectory(targetDir, zipFile, CompressionLevel.Fastest, false);
		}
	}
}