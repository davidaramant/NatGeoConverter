using DataModel;
using DataModel.Html;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Website_Generator {
	class Program {
		readonly static string _basePath = GetDrivePath();
		readonly static string _baseJpgPath = Path.Combine( _basePath, "JPG" );
		readonly static string _baseHtmlPath = Path.Combine( _basePath, "HTML" );

		static string GetDrivePath() {
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

		// TODO: Getting rid of special page handling does not reliably find the cover image
		static void DoStuff() {
			//var decades = GenerateModel();
			var decades = Deserialize( "serialized.txt" );

			WL( "{0} decades", decades.Count() );

			//GenerateThumbnails( decades );
			GenerateMainIndex( decades );
			//GenerateDecadeIndexes( decades );
			//GeneratePageIndexes( decades );
		}

		private static IEnumerable<NGDecade> GenerateModel() {
			var decades =
				Directory.GetDirectories( _baseJpgPath ).
                Select( decadeDir => NGDecade.Parse( decadeDir, basePath: _basePath ) ).
                OrderBy( decade => decade.Name ).
                ToArray();

			Serialize( decades, "serialized.txt" );

			return decades;
		}

		private static void Serialize( IEnumerable<NGDecade> decades, string path ) {
			using( var serialized = File.CreateText( path ) ) {
				foreach( var decade in decades ) {
					serialized.WriteLine( decade.Serialize() );
					foreach( var issue in decade ) {
						serialized.WriteLine( issue.Serialize() );
						foreach( var page in issue ) {
							serialized.WriteLine( page.Serialize() );
						}
					}
				}
			}
		}

		private static IEnumerable<NGDecade> Deserialize( string path ) {
			var decades = new List<NGDecade>();

			string decadeName = String.Empty;
			DateTime issueDate = DateTime.Now;
			var issues = new List<NGIssue>();
			var pages = new List<NGPage>();

			foreach( var line in File.ReadAllLines( path ) ) {
				var parts = line.Split( ';' );

				switch( parts[ 0 ] ) {
					case "decade":
						if( pages.Any() ) {
							issues.Add( new NGIssue( pages, issueDate ) );
							pages.Clear();
						}
						if( issues.Any() ) {
							decades.Add( new NGDecade( issues, decadeName ) );
							issues.Clear();
						}

						decadeName = parts[ 1 ];
						break;
					case "issue":
						if( pages.Any() ) {
							issues.Add( new NGIssue( pages, issueDate ) );
							pages.Clear();
						}

						issueDate = DateTime.ParseExact( parts[ 1 ], "s", CultureInfo.InvariantCulture );
						break;
					case "page":
						pages.Add( NGPage.Parse( Path.Combine( _basePath, parts[ 1 ] ), _basePath ) );
						break;
				}

			}

			if( pages.Any() ) {
				issues.Add( new NGIssue( pages, issueDate ) );
				pages.Clear();
			}
			if( issues.Any() ) {
				decades.Add( new NGDecade( issues, decadeName ) );
				issues.Clear();
			}

			return decades;
		}

		static void GenerateThumbnails( IEnumerable<NGDecade> decades ) {
			foreach( var decade in decades ) {
				foreach( var issue in decade ) {
					foreach( var page in issue ) {
						var thumbPath = Path.Combine( _basePath, "thumbnails", page.RelativePath );

						using( var image = Image.FromFile( page.FullPath ) ) {
							//image.GetThumbnailImage( thumbHeight: 400, thumbWidth: 400 );
						}
					}
				}
			}
		}

		static void GenerateMainIndex( IEnumerable<NGDecade> decades ) {
									var sw = new StringWriter();
			using( var index = new HtmlWriter( sw, "National Geographic" ) ) {
				using( var previews = index.Div( className: "previews" ) ) {
					foreach( var decade in decades.OrderBy( _ => _.Name ) ) {
						using( var decadePreview = previews.Div( "previewBox" ) ) {
							using( var decadeLink = decadePreview.Link( Path.Combine( "web", decade.Name, decade.Name + ".html" ) ) ) {
								decadeLink.Img( link: decade.First().Cover.RelativePath, altText: decade.Name );
								decadeLink.H2( decade.Name );
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
				using( var index = new HtmlWriter( sw, title: decade.Name, pathModifier: Path.Combine( "..", ".." ) ) ) {
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

				var path = Path.Combine( _basePath, "web", decade.Name );

				if( !Directory.Exists( path ) ) {
					Directory.CreateDirectory( path );
				}

				File.WriteAllText(
					Path.Combine( path, decade.Name + ".html" ),
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

					var path = Path.Combine( _basePath, "web", decade.Name, issue.Name );

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
