using DataModel;
using DataModel.Html;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Diagnostics;

namespace Website_Generator {
	class Program {
		readonly static string _basePath = GetBasePath();
		readonly static string _baseFullImagePath = Path.Combine( _basePath, "imgs", "full" );
		readonly static string _baseThumbnailPath = Path.Combine( _basePath, "imgs", "thumbs" );
		readonly static string _baseHtmlPath = Path.Combine( _basePath, "html" );

		static string GetBasePath() {
			if( Environment.OSVersion.Platform == PlatformID.Win32NT ) {
				return "G:";
			}
			return Path.Combine( "/", "Users", "davidaramant", "Web", "NatGeo" );
		}

		static void Main( string[] args ) {
			try {
				DoStuff();
			} catch( Exception e ) {
				WL( new String( '-', 79 ) );
				WL( e.ToString() );
			}

			WL( "Done" );
			Console.ReadKey();
		}

		static void WL() {
			Console.Out.WriteLine();
		}

		static void WL( string format, params object[] args ) {
			Console.Out.WriteLine( format, args );
		}

		private static string CsvLine( params object[] columns ) {
			return String.Join( ",", columns.Select( c => "\"" + c + "\"" ) );
		}
			
		static void DoStuff() {
			var timer = Stopwatch.StartNew();
			var decades = GenerateModel();
			WL( "Generating model took: " + timer.Elapsed );

			WL( "{0} decades", decades.Count() );

			timer.Restart();
			//GenerateThumbnails( decades );
			//WL( "Thumbnail generation took: " + timer.Elapsed );

			//GenerateMainIndex( decades );
			//GenerateDecadeIndexes( decades );
			//GeneratePageIndexes( decades );
		}

		private static IEnumerable<NGDecade> GenerateModel() {
			var decades =
				Directory.GetDirectories( _baseFullImagePath ).
				Select( decadeDir => NGDecade.Parse( path:decadeDir, basePath: _basePath ) ).
                OrderBy( decade => decade.DisplayName ).
                ToArray();

			return decades;
		}

		static void CreatePath( string path ) {
			var dirPart = Path.GetDirectoryName( path );
			if( !Directory.Exists( dirPart ) ) {
				Directory.CreateDirectory( dirPart );
			}
		}

		static void GenerateThumbnails( IEnumerable<NGDecade> decades ) {
			foreach( var decade in decades ) {
				WL( "Decade: {0} {1}", decade.DisplayName, DateTime.Now.ToString("s") );
				foreach( var issue in decade ) {
					foreach( var page in issue ) {
						CreatePath( page.NormalThumbnailFullPath );

						using( var p1 = StartGeneratingThumbnail( page.FullPath, page.NormalThumbnailFullPath, 180 ) )
						using( var p2 = StartGeneratingThumbnail( page.FullPath, page.RetinaThumbnailFullPath, 360 ) ) {
							p1.WaitForExit();
							p2.WaitForExit();
						}
					}
				}
			}
		}

		static Process StartGeneratingThumbnail( string inputPath, string outputPath, int size ) {
			var processInfo = new System.Diagnostics.ProcessStartInfo {
				FileName = "convert",
				Arguments = String.Format("\"{0}\" -resize {1} -quality 100% \"{2}\"", inputPath, size, outputPath ),
				CreateNoWindow = true,
			};

			return System.Diagnostics.Process.Start( processInfo );
		}

		static void GenerateMainIndex( IEnumerable<NGDecade> decades ) {
									var sw = new StringWriter();
			using( var index = new HtmlWriter( sw, "National Geographic" ) ) {
				using( var previews = index.Div( className: "previews" ) ) {
					foreach( var decade in decades.OrderBy( _ => _.DisplayName ) ) {
						using( var decadePreview = previews.Div( "previewBox" ) ) {
							using( var decadeLink = decadePreview.Link( Path.Combine( "web", decade.DisplayName, decade.DisplayName + ".html" ) ) ) {
								decadeLink.Img( link: decade.First().Cover.RelativePath, altText: decade.DisplayName );
								decadeLink.H2( decade.DisplayName );
							}
						}
					}
				}
			}

			File.WriteAllText( Path.Combine( _basePath, "index.html" ), sw.ToString(), System.Text.Encoding.UTF8 );
		}

		static void GenerateDecadeIndexes( IEnumerable<NGDecade> decades ) {
			foreach( var decade in decades ) {

				var sw = new StringWriter();
				using( var index = new HtmlWriter( sw, title: decade.DisplayName, pathModifier: Path.Combine( "..", ".." ) ) ) {
					using( var previews = index.Div( className: "previews" ) ) {
						foreach( var issue in decade ) {
							using( var issuePreview = previews.Div( "previewBox" ) ) {
								using( var previewLink = issuePreview.Link( Path.Combine( issue.Name, issue.Name + ".html" ) ) ) {
									previewLink.Img(
										link: Path.Combine( "..", "..", issue.Cover.RelativePath ),
										altText: issue.DisplayName );
									previewLink.H2( issue.DisplayName );
								}
							}
						}
					}
				}

				var path = Path.Combine( _basePath, "web", decade.DisplayName );

				if( !Directory.Exists( path ) ) {
					Directory.CreateDirectory( path );
				}

				File.WriteAllText(
					Path.Combine( path, decade.DisplayName + ".html" ),
					sw.ToString(),
					System.Text.Encoding.UTF8 );
			}
		}

		static void GeneratePageIndexes( IEnumerable<NGDecade> decades ) {
			var pathModifier = Path.Combine( "..", "..", ".." );

			foreach( var decade in decades ) {
				foreach( var issue in decade ) {
					var sw = new StringWriter();
					using( var index = new HtmlWriter( sw, title: issue.DisplayName, pathModifier: pathModifier ) ) {
						using( var previews = index.Div( className: "previews" ) ) {
							foreach( var page in issue ) {
								using( var pagePreview = previews.Div( "previewBox" ) ) {

									using( var previewLink = pagePreview.Link( page.DisplayName + ".html" ) ) {
										previewLink.Img(
											link: Path.Combine( pathModifier, page.RelativePath ),
											altText: page.DisplayName );
										previewLink.H2( page.DisplayName );
									}
								}
							}
						}
					}

					var path = Path.Combine( _basePath, "web", decade.DisplayName, issue.Name );

					if( !Directory.Exists( path ) ) {
						Directory.CreateDirectory( path );
					}

					File.WriteAllText(
						Path.Combine( path, issue.Name + ".html" ),
						sw.ToString(),
						System.Text.Encoding.UTF8 );
				}
			}
		}
	}
}
