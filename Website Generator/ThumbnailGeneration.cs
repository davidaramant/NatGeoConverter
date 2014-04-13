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
			var allJpgs = Directory.GetFiles( config.AbsoluteFullImageDir, "*.jpg", SearchOption.AllDirectories );
			Out.WL( "Reading all paths took: {0}", timer.Elapsed );

			const int batchSize = 1000;
			var numBatches = (allJpgs.Length / batchSize) + 1;

			var startTime = DateTime.Now;
			Out.WL( "Total # images: {0}\tStart Time: {1}", allJpgs.Length, startTime);

			var batchTimer = new Stopwatch();
			foreach( var batch in allJpgs.InBatchesOf( batchSize ).Select( (paths,index)=>new{Images = paths, Number = index + 1 } )) {
				batchTimer.Reset();
				batchTimer.Start();
				Out.WL( "Batch {0}/{1}", batch.Number, numBatches );
				foreach( var fullImagePath in batch.Images ) {
					var thumbPath = ConvertFullPathToThumbnailPath( config, fullImagePath );

					Utility.CreateDirInFilePath( thumbPath );

					using( var p = StartGeneratingThumbnail( fullImagePath, thumbPath, config.ThumbnailSize ) ) {
						p.WaitForExit();
					}
				}
				Out.WL( "Done with batch, took: {0}\tEstimated done: {1}", 
					batchTimer.Elapsed, 
					startTime + TimeSpan.FromTicks( numBatches*batchTimer.ElapsedTicks ) );
			}
		}

		static Process StartGeneratingThumbnail( string inputPath, string outputPath, Size maxSize ) {
			var arguments = String.Format( "\"{0}\" -resize {1}x{2} -quality 50% \"{3}\"", 
				                inputPath, maxSize.Width, maxSize.Height, outputPath );

			var processInfo = new System.Diagnostics.ProcessStartInfo {
				FileName = "/usr/local/bin/convert",
				Arguments = arguments,
				CreateNoWindow = true,
				UseShellExecute = false,
			};

			return System.Diagnostics.Process.Start( processInfo );
		}

		static string ConvertFullPathToThumbnailPath( IProjectConfig config, string fullImagePath )
		{
			var relativePath = fullImagePath.GetPathRelativeTo( config.AbsoluteFullImageDir );
			return Path.Combine( config.AbsoluteThumbnailImageDir, relativePath );
		}
	}
}

