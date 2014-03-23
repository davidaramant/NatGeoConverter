using DataModel;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Text;
using Utilities.EnumerableExtensions;
using Utilities;

namespace Website_Generator {
	class Program {
		readonly static string _basePath = GetBasePath();
		readonly static string _baseFullImagePath = Path.Combine( _basePath, "imgs", "full" );
		readonly static string _baseThumbnailPath = Path.Combine( _basePath, "imgs", "thumbs" );

		static string GetBasePath() {
			if( Environment.OSVersion.Platform == PlatformID.Win32NT ) {
				return "G:";
			}
			return Path.Combine( "/", "Users", "davidaramant", "Web", "NatGeo" );
		}

		static void Main( string[] args ) {
			try {
				DoStuff( args );
			} catch( Exception e ) {
				Out.WL( new String( '-', 79 ) );
				Out.WL( e.ToString() );
			}

			Out.WL( "Done" );
		}

		static void DoStuff( string[] args ) {
			var config = new ProjectConfig( baseDir: Path.Combine( "/", "Users", "davidaramant", "Web", "NatGeo" ) );

			ThumbnailGeneration.GenerateThumbnails( config );
			return;


			var timer = Stopwatch.StartNew();
			var decades = GenerateModel();
			Out.WL( "Generating model took: " + timer.Elapsed );

			Out.WL( "{0} decades", decades.Count() );

			timer.Restart();

			//GenerateMainIndex( decades );
			//GenerateDecadeIndexes( decades );
			//GenerateIssueIndexes( decades );
			GeneratePageIndexes( decades );
			Out.WL( "HTML generation took: " + timer.Elapsed );
		}

		private static IEnumerable<NGDecade> GenerateModel() {
			return Enumerable.Empty<NGDecade>();
		}

		static void GenerateMainIndex( IEnumerable<NGDecade> decades ) {
			var sb = new StringBuilder();
			sb.Append( GetHeader( depth: 0, title: "The Complete National Geographic" ) );
			sb.Append( GetNavBar( NamedLink.Empty( "Decades" ) ) );

			sb.Append( @"<div class=""container""> " );

			foreach( var batch in decades.GetBatchesOfSize( 4 ) ) {
				sb.AppendLine( @"<div class=""row"">" );
				foreach( var decade in batch ) {
					sb.AppendFormat( 
						GetThumbnailHtml(
							displayName: decade.DisplayName,
							previewText: String.Format( "Decade preview for {0}", decade.DisplayName ),
							imgUrl: decade.PreviewImagePath,
							linkUrl: decade.RelativeIndexUrl ) );
				}
				sb.AppendLine( @"</div>" );
			}


			sb.AppendLine( @"</div> <!-- container --> " );
			sb.AppendLine( GetFooter( depth: 0 ) );

			File.WriteAllText( Path.Combine( _basePath, "index.html" ), sb.ToString(), Encoding.UTF8 );
		}

		static string GetThumbnailHtml( string displayName, string previewText, string imgUrl, string linkUrl ) {
			return String.Format( 
				@"<div class=""col-md-3 col-sm-3 col-sx-3"">
							<a href=""{3}"">
								<div class=""panel panel-default"">
									<div class=""panel-heading"">
										<h3 class=""panel-title"">{0}</h3>
									</div>
									<div class=""panel-body"">
										<img src=""{2}"" class=""img-thumbnail center-block"" alt=""{1}""/>
									</div>
								</div>
							</a>
						</div>", displayName, previewText, imgUrl, linkUrl );
		}

		static string GetHeader( int depth, string title, bool smallerBodyPadding = false ) {
			var modifier = Path.Combine( Enumerable.Repeat( "..", depth ).ToArray() );

			return String.Format( @"<!DOCTYPE html>
<html lang=""en"">
  <head>
    <meta charset=""utf-8"">
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
    <title>{0}</title>
    <link href=""{1}"" rel=""stylesheet"">
    <link href=""{2}"" rel=""stylesheet"">
	<link rel=""shortcut icon"" href=""{3}"">
  </head>
  <body {4}>", title,
				Path.Combine( modifier, "css", "bootstrap.min.css" ),
				Path.Combine( modifier, "css", "customizations.css" ),
				Path.Combine( modifier, "favicon_v04.ico" ),
				smallerBodyPadding ? @"style=""padding-top: 60px;""" : String.Empty );
		}

		sealed class NamedLink {
			private readonly string _url;
			private readonly string _name;

			public string Name { get { return _name; } }

			public string Url { get { return _url; } }

			public bool HasUrl { get { return !String.IsNullOrWhiteSpace( Url ); } }

			public NamedLink( string name, string url ) {
				_name = name;
				_url = url;
			}

			public static NamedLink Empty( string name ) {
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
					sb.AppendFormat( @"<li><a href=""{1}"">{0}</a></li>", link.Name, link.Url );
				} else {
					sb.AppendFormat( @"<li class=""active"">{0}</li>", link.Name );
				}
			}

			sb.Append( @"
									</ul>
								</ul>
							</div>
						</div>
					</div>" ); 

			return sb.ToString();
		}

		static string GetNavBarWithButtons( NamedLink previous, NamedLink next, params NamedLink[] links ) {
			var sb = new StringBuilder();
			sb.Append( 
				@"<div class=""navbar navbar-default navbar-fixed-top"" role=""navigation"">  
						<div class=""container"">
							<div class=""navbar-collapse collapse"">
								<ul class=""nav navbar-nav"">
									<ul class=""breadcrumb list-inline"">" );

			foreach( var link in links ) {
				if( link.HasUrl ) {
					sb.AppendFormat( @"<li><a href=""{1}"">{0}</a></li>", link.Name, link.Url );
				} else {
					sb.AppendFormat( @"<li class=""active"">{0}</li>", link.Name );
				}
			}

			sb.AppendFormat( @"</ul>
								</ul>
								<div class=""nav navbar-right"">
									<div class=""btn-group"">
  										{0}
										<button class=""btn navbar-btn btn-default"" onClick=""toggleHorizontal();""><span class=""glyphicon glyphicon glyphicon-resize-horizontal""/></button>
										<button class=""btn navbar-btn btn-default"" onClick=""toggleVertical();""><span class=""glyphicon glyphicon glyphicon-resize-vertical""/></button>  										
										{1}
									</div>
								</div>
							</div>
						</div>
					</div>", 
				CreateNavButton( left: true, link: previous ),
				CreateNavButton( left: false, link: next ) ); 
				
			return sb.ToString();
		}

		static string CreateNavButton( bool left, NamedLink link ) {
			var direction = left ? "left" : "right";
			if( link == null ) {
				return String.Format( 
					@"<button class=""btn navbar-btn btn-default btn-page"" disabled=""disabled""><span class=""glyphicon glyphicon-chevron-{0}""/></button>", 
					direction );
			} else {
				return String.Format( 
					@"<a class=""btn navbar-btn btn-default btn-page"" href=""{1}""><span class=""glyphicon glyphicon-chevron-{0}""/></a>", 
					direction, link.Url );
			}
		}

		static string GetFooter( int depth, bool imageSizeToggles = false ) {
			var modifier = Path.Combine( Enumerable.Repeat( "..", depth ).ToArray() );

			var javascriptFiles = new List<string> {
				"jquery.min.js",
				"bootstrap.min.js",
			};
			if( imageSizeToggles ) {
				javascriptFiles.Add( "imageFitToggles.js" );
			}

			var footer = new StringBuilder();
			foreach( var file in javascriptFiles ) {
				footer.AppendFormat( @"<script src=""{0}""></script>",
					Path.Combine( modifier, "js", file ) );
			}
			footer.Append( @"</body></html>" );
			return footer.ToString();
		}

		static void GenerateDecadeIndexes( IEnumerable<NGDecade> decades ) {
			foreach( var decade in decades ) {
				var sb = new StringBuilder();
				sb.Append( GetHeader( depth: 1, title: "NatGeo: The " + decade.DisplayName ) );
				sb.Append( GetNavBar( new NamedLink( "Decades", Path.Combine( "..", "index.html" ) ),
					NamedLink.Empty( decade.DisplayName ) ) );

				sb.Append( @"<div class=""container""> " );

				foreach( var yearGroup in decade.GroupBy( d => d.ReleaseDate.Year ).OrderBy( yearGroup => yearGroup.First().ReleaseDate ) ) {
					sb.AppendFormat( @"<div class=""panel panel-primary""><div class=""panel-heading""><h2 class=""pabel-title"">{0}</h2></div><div class=""panel-body"">", 
						yearGroup.First().ReleaseDate.Year );

					foreach( var batch in yearGroup.OrderBy( issue => issue.ReleaseDate ).GetBatchesOfSize( 4 ) ) {
						sb.AppendLine( @"<div class=""row"">" );
						foreach( var issue in batch ) {
							sb.Append( GetThumbnailHtml(
								displayName: issue.ShortDisplayName,
								previewText: String.Format( "Preview for {0}", issue.ShortDisplayName ),
								imgUrl: Path.Combine( "..", issue.Cover.RelativeThumbnailUrl ),
								linkUrl: Path.Combine( decade.DirectoryName, issue.RelativeIndexUrl ) ) );
						}
						sb.AppendLine( @"</div>" ); // row
					}
					sb.AppendLine( @"</div></div>" ); // panel body, panel
				}


				sb.AppendLine( @"</div> <!-- container --> " );

				sb.Append( GetFooter( depth: 1 ) );

				File.WriteAllText( Path.Combine( _basePath, "html", decade.IndexFileName ), sb.ToString(), Encoding.UTF8 );
			}
		}

		static void GenerateIssueIndexes( IEnumerable<NGDecade> decades ) {
			foreach( var decade in decades ) {
				foreach( var issue in decade ) {
					var sb = new StringBuilder();
					sb.Append( GetHeader( depth: 3, title: "NatGeo: " + issue.LongDisplayName ) );
					sb.Append( GetNavBar( 
						new NamedLink( "Decades", Path.Combine( "..", "..", "..", "index.html" ) ), 
						new NamedLink( decade.DisplayName, Path.Combine( "..", "..", decade.IndexFileName ) ),
						NamedLink.Empty( issue.LongDisplayName ) ) );

					sb.Append( @"<div class=""container""> " );

					foreach( var batch in issue.GetBatchesOfSize(4) ) {
						sb.Append( @"<div class=""row"">" );
						foreach( var page in batch ) {
							sb.Append( GetThumbnailHtml(
								displayName: page.DisplayName,
								previewText: String.Format( "Preview for {0}", page.DisplayName ),
								imgUrl: Path.Combine( "..", "..", "..", page.RelativeThumbnailUrl ),
								linkUrl: page.IndexName ) );
						}
						sb.AppendLine( @"</div>" );
					}

					sb.AppendLine( @"</div> <!-- container --> " );

					sb.Append( GetFooter( depth: 3 ) );

					var path = Path.Combine( _basePath, "html", decade.DirectoryName, issue.RelativeIndexUrl );

					Utility.CreatePath( path );

					File.WriteAllText( path, sb.ToString(), Encoding.UTF8 );
				}
			}
		}

		static void GeneratePageIndexes( IEnumerable<NGDecade> decades ) {
			var pathModifier = Path.Combine( "..", "..", ".." );

			foreach( var decade in decades ) {
				foreach( var issue in decade ) {
					for( int pageIndex = 0; pageIndex < issue.Count; pageIndex++ ) {
						NGPage previousPage = (pageIndex == 0) ? null : issue[ pageIndex - 1 ];
						NGPage nextPage = (pageIndex == (issue.Count - 1)) ? null : issue[ pageIndex + 1 ];

						var currentPage = issue[ pageIndex ];

						var sb = new StringBuilder();
						sb.Append( GetHeader( 
							depth: 3, 
							title: String.Format( "NatGeo: {0} - {1}", issue.LongDisplayName, currentPage.DisplayName ), 
							smallerBodyPadding: true ) );
						sb.Append( GetNavBarWithButtons(
							previous: MakeLinkToPage( previousPage ),
							next: MakeLinkToPage( nextPage ),
							links: new[] {
								new NamedLink( "Decades", Path.Combine( "..", "..", "..", "index.html" ) ), 
								new NamedLink( decade.DisplayName, Path.Combine( "..", "..", decade.IndexFileName ) ),
								new NamedLink( issue.LongDisplayName, issue.IndexFileName ),
								NamedLink.Empty( String.Format( "{0} of {1}", currentPage.DisplayName, issue.Count ) )
							} ) );
						sb.Append( @"<div class=""container-fluid"" style=""padding: 0;""> " );

						sb.AppendFormat( @"<img id=""pageScan"" src=""{0}"" alt=""{1}"" class=""img-responsive center-block""/>",
							Path.Combine( "..", "..", "..", currentPage.RelativeFullUrl ), currentPage.DisplayName );

						sb.AppendLine( @"</div>" );

						sb.Append( GetFooter( depth: 3, imageSizeToggles: true ) );

						var path = Path.Combine( _basePath, "html", decade.DirectoryName, issue.DirectoryName, currentPage.IndexName );

						File.WriteAllText( path, sb.ToString(), Encoding.UTF8 );
					} // page
				} // issue
			} // decade
		}

		static NamedLink MakeLinkToPage( NGPage page ) {
			if( page == null )
				return null;
			return new NamedLink( name: page.DisplayName, url: page.IndexName );
		}
	}
}
