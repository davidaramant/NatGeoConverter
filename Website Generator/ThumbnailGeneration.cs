using System;
using DataModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Utilities;
using System.IO;
using Utilities.PathExtensions;
using Utilities.EnumerableExtensions;
using System.Linq;

namespace Website_Generator {
	public static class ThumbnailGeneration {
		public static void GenerateThumbnails( IProjectConfig config ) {
			var timer = Stopwatch.StartNew();
			var allJpgs = Directory.GetFiles( config.BaseFullImageDir, "*.jpg", SearchOption.AllDirectories );
			Out.WL( "Reading all paths took: {0}", timer.Elapsed );

			const int batchSize = 10000;
			var numBatches = (allJpgs.Length / batchSize) + 1;

			Out.WL( "Total # images: {0}", allJpgs.Length );

			foreach( var batch in allJpgs.GetBatchesOfSize( batchSize ).Select( (paths,index)=>new{Images = paths, Number = index + 1 } )) {
				timer.Reset();
				Out.WL( "Batch {0}/{1}", batch.Number, numBatches );
				foreach( var fullImagePath in batch.Images ) {
					var thumbPath = ConvertFullPathToThumbnailPath( config, fullImagePath );

					Utility.CreatePath( thumbPath );

					using( var p2 = StartGeneratingThumbnail( fullImagePath, thumbPath, config.ThumbnailSize ) ) {
						p2.WaitForExit();
					}
				}
				Out.WL( "Done with batch, took: {0}", timer.Elapsed );
			}
		}

		static Process StartGeneratingThumbnail( string inputPath, string outputPath, Size maxSize ) {
			var processInfo = new System.Diagnostics.ProcessStartInfo {
				FileName = "convert",
				Arguments = String.Format( "\"{0}\" -resize {1}x{2} -quality 50% \"{3}\"", 
					inputPath, maxSize.Width, maxSize.Height, outputPath ),
				CreateNoWindow = true,
			};

			return System.Diagnostics.Process.Start( processInfo );
		}

		static string ConvertFullPathToThumbnailPath( IProjectConfig config, string fullImagePath )
		{
			var relativePath = fullImagePath.GetPathRelativeTo( config.BaseFullImageDir );
			return Path.Combine( config.BaseThumbnailImageDir, relativePath );
		}
	}
}

