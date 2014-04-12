using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Utilities.EnumerableExtensions;

namespace National_Geographic_Converter {
	public static class CngConverter {
		public static void ConvertAllInPath( string inputPath, string outputPath ) {
			var allFiles = Directory.EnumerateFiles( inputPath, "*.cng", SearchOption.AllDirectories ).ToArray();

			var numberDone = 0;
			var totalNumber = allFiles.Length;

			foreach( var batch in allFiles.InBatchesOf( 500 ) ) {
				var oldAndNewNames = batch.Select( name => new { 
					Old = name, 
					New = GetOutputFilePath( name, outputPath ) } ).ToArray();

				var loadedFiles =
					from namePair in oldAndNewNames
						where !File.Exists( namePair.New )
					let data = File.ReadAllBytes( namePair.Old )
					select new {
					OutputName = namePair.New,
					Data = data,
				};

				var fixedDataFiles =
					loadedFiles.AsParallel().Select( loadedFile => new { 
						Name = loadedFile.OutputName, 
						Data = loadedFile.Data.Select( b => (byte)( b ^ 0xEF ) ).ToArray() } );

				foreach( var fixedFile in fixedDataFiles ) {
					File.WriteAllBytes( fixedFile.Name, fixedFile.Data );
				}

				numberDone += oldAndNewNames.Length;
				WL( "Done with: {0} of {1}", numberDone, totalNumber );
			}
		}

		private static void WL( string format, params object[] args ) {
			Console.Out.WriteLine( format, args );
		}


		private static string GetOutputFilePath( string inputFilePath, string outputPath ) {
			var components = inputFilePath.Split( new[] { Path.DirectorySeparatorChar } ).ToArray();

			var decadePath = components[components.Length - 3];
			var issuePath = components[components.Length - 2];
			var fileName = Path.GetFileNameWithoutExtension( components[components.Length - 1] );

			var fullEraPath = Path.Combine( outputPath, decadePath );

			if( !Directory.Exists( fullEraPath ) ) {
				Directory.CreateDirectory( fullEraPath );
			}

			var fullIssuePath = Path.Combine( fullEraPath, issuePath );

			if( !Directory.Exists( fullIssuePath ) ) {
				Directory.CreateDirectory( fullIssuePath );
			}

			return Path.Combine( fullIssuePath, fileName + ".jpg" );
		}
	}
}

