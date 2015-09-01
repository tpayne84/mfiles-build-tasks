using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using VAFPackageEngine;

namespace MFiles.BuildTasks
{
	/// <summary>
	/// VAF Module Packaging Task.
	/// </summary>
	public class ModulePack : Task
	{
		public ModulePack()
		{
			this.BuildEngine = base.BuildEngine;
		}

		/// <summary>
		/// Solution File Path ( In Same Directory as Build Configuration of Same Name ... with .json extension )
		/// </summary>
		[Required]
		public string PathToSolutionFile { get; set; }
	
		/// <summary>
		/// Package Builder config.json Full Path
		/// </summary>
		[Required]
		public string PathToBuilderConfiguration { get; set; }
		
		/// <summary>
		/// Packaging Logic
		/// </summary>
		/// <returns></returns>
		public override bool Execute()
		{
			Engine e = new Engine(PathToSolutionFile);

			if( File.Exists( PathToBuilderConfiguration ) )
			{
				e.Configuration = JsonConvert.DeserializeObject<VAFPackageConfiguration>( File.ReadAllText( PathToBuilderConfiguration ) );

				e.Verbose = true;
				try
				{
					e.Package();
					return true;
				}
				catch (Exception)
				{
					return false;
				}
			}

			return false;
		}
	}
}