using System;
using DataModel;
using System.Collections.Generic;
using System.Diagnostics;

namespace Website_Generator {
	public static class ThumbnailGeneration {
		static IEnumerable<NGDecade> GetSubSet( IEnumerable<NGDecade> decades, string startDecade, string endDecade ) {
			bool foundStart = false;
			foreach( var decade in decades ) {
				if( !foundStart ) {
					if( decade.DisplayName == startDecade ) {
						foundStart = true;
						yield return decade;
					}
				} else {
					yield return decade;

					if( decade.DisplayName == endDecade ) {
						yield break;
					}
				}
			}
		}

		public static void GenerateThumbnails( IEnumerable<NGDecade> decades, string startDecade, string endDecade ) {
			var subSet = GetSubSet( decades, startDecade, endDecade );

			foreach( var decade in subSet ) {
				Console.Out.WriteLine( "Decade: {0} {1}", decade.DisplayName, DateTime.Now.ToString( "s" ) );
				foreach( var issue in decade ) {
					foreach( var page in issue ) {
						throw new NotImplementedException( "Fix up these paths using IProjectConfig" );
						Utility.CreatePath( "thumbnail full path" );

						using( var p2 = StartGeneratingThumbnail( "fullPath", "thumbnailpath", 360, 520 ) ) {
							p2.WaitForExit();
						}
					}
				}
			}
		}

		static Process StartGeneratingThumbnail( string inputPath, string outputPath, int xSize, int ySize ) {
			var processInfo = new System.Diagnostics.ProcessStartInfo {
				FileName = "convert",
				Arguments = String.Format( "\"{0}\" -resize {1}x{2} -quality 100% \"{3}\"", inputPath, xSize, ySize, outputPath ),
				CreateNoWindow = true,
			};

			return System.Diagnostics.Process.Start( processInfo );
		}
	}
}

