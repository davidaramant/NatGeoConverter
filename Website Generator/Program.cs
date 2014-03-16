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
using System.Text;
using DataModel.Extensions;

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
			GenerateDecadeIndexes( decades );
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
			var sb = new StringBuilder();
			sb.Append( GetHeader(depth:0,title:"The Complete National Geographic"));
			sb.Append( GetNavBar( NamedLink.Empty( "Decades" ) ) );

			sb.Append(@"<div class=""container""> ");

			foreach( var batch in decades.GetBatchesOfSize( 4 ) ) {
				sb.AppendLine( @"<div class=""row"">" );
				foreach( var decade in batch ) {
					sb.AppendFormat( 
						@"<div class=""col-md-3 col-sm-3 col-sx-3"">
							<a href=""{2}"">
								<img src=""{1}"" class=""img-thumbnail"" alt=""Decade preview for {0}""/>
								<h3>{0}</h3>
							</a>
						</div>", decade.DisplayName, decade.PreviewImagePath, decade.RelativeIndexUrl );
				}
				sb.AppendLine( @"</div>" );
			}


			sb.AppendLine(@"</div> <!-- container --> " );
			sb.AppendLine(GetFooter(depth:0));

			File.WriteAllText( Path.Combine( _basePath, "index.html" ), sb.ToString(), Encoding.UTF8 );
		}

		static string GetHeader( int depth, string title ) {
			var modifier = Path.Combine(Enumerable.Repeat( "..", depth ).ToArray());

			return String.Format(@"<!DOCTYPE html>
<html lang=""en"">
  <head>
    <meta charset=""utf-8"">
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
    <title>{0}</title>
    <link href=""{1}"" rel=""stylesheet"">
    <link href=""{2}"" rel=""stylesheet"">
  </head>
  <body>", title,
				Path.Combine(modifier,"css","bootstrap.min.css"),
				Path.Combine(modifier,"css","customizations.css") );
		}

		sealed class NamedLink
		{
			private readonly string _url;
			private readonly string _name;

			public string Name { get { return _name;}}
			public string Url { get { return _url; } }
		
			public bool HasUrl { get { return !String.IsNullOrWhiteSpace( Url ); } }

			public NamedLink( string name, string url )
			{
				_name = name;
				_url = url;
			}

			public static NamedLink Empty( string name )
			{
				return new NamedLink( name: name, url: null );
			}
		}

		static string GetNavBar( params NamedLink[] links ) {
			var sb = new StringBuilder();
			sb.Append( 
				@"<div class=""navbar navbar-default navbar-fixed-top"" role=""navigation"">  
						<div class=""container"">
							<div class=""navbar-collapse collapse"">
								<ul class=""nav navbar-nav"">
									<ul class=""breadcrumb list-inline"">" );

			foreach( var link in links ) {
				if( link.HasUrl ) {
					sb.AppendFormat( @"@<li><a href=""{1}"">{0}</a></li>", link.Name, link.Url );
				} else {
					sb.AppendFormat( @"<li class=""active"">{0}</li>", link.Name );
				}
			}

			sb.Append(@"
									</ul>
								</ul>
							</div>
						</div>
					</div>" ); 

			return sb.ToString();
		}

		static string GetFooter(int depth) {
			var modifier = Path.Combine(Enumerable.Repeat( "..", depth ).ToArray());

			return String.Format(
				@"<script src=""{0}""></script>
			      <script src=""{1}""></script>
			      <script src=""{2}""></script>
  </body>
</html>",
				Path.Combine(modifier,"js","jquery.min.js"),
				Path.Combine(modifier, "js","bootstrap.min.js"),
				Path.Combine(modifier, "js", "retina.min.js") );
		}

		static void GenerateDecadeIndexes( IEnumerable<NGDecade> decades ) {
			foreach( var decade in decades ) {
				var sb = new StringBuilder();
				sb.Append( GetHeader( depth:1, title:"The " + decade.DisplayName ) );
				sb.Append( GetNavBar( new NamedLink("Decades", Path.Combine("..","index.html") ), NamedLink.Empty( decade.DisplayName ) ) );

				sb.Append(@"<div class=""container""> ");

				foreach( var yearGroup in decade.GroupBy( d => d.ReleaseDate.Year ).OrderBy( yearGroup => yearGroup.First().ReleaseDate ) ) {
					sb.AppendFormat( @"<div class=""page-header""><h1>{0}</h1></div>", yearGroup.First().ReleaseDate.Year );

					foreach( var batch in yearGroup.OrderBy( issue => issue.ReleaseDate ).GetBatchesOfSize( 4 ) ) {
						sb.AppendLine( @"<div class=""row"">" );
						foreach( var issue in batch ) {
							sb.AppendFormat( 
								@"<div class=""col-md-3 col-sm-3 col-sx-3"">
							<a href=""{2}"">
								<img src=""{1}"" class=""img-thumbnail"" alt=""Preview for {0}""/>
								<h3>{0}</h3>
							</a>
						</div>", issue.ReleaseDate.ToString("MMMM d"), Path.Combine("..", issue.Cover.NormalThumbnailUrl ), issue.RelativeIndexUrl );
						}
						sb.AppendLine( @"</div>" );
					}
				}


				sb.AppendLine(@"</div> <!-- container --> " );

				sb.Append( GetFooter( depth: 1 ) );

				File.WriteAllText( Path.Combine( _basePath, "html", decade.IndexFileName ), sb.ToString(), Encoding.UTF8 );
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
