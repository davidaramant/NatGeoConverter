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
using Website_Generator.Models;

namespace Website_Generator {
	class Program {
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

			var model = new NGCollection( config );
			var timer = Stopwatch.StartNew();

			//GenerateMainIndex( config, model );
			//GenerateDecadeIndexes( config, model );
			//GenerateYearIndexes( config, model );
			//GenerateIssueIndexes( config, model );
			GeneratePageIndexes( config, model );

			Out.WL( "HTML generation took: " + timer.Elapsed );
		}

		static void GenerateMainIndex( IProjectConfig config, NGCollection ngCollection ) {
			var template = new SiteLayout() { 
				Model = new SiteLayoutModel( 
					config: config, 
					pageTitle: "The Complete National Geographic", 
					depth: 0, 
					bodyModel: new MainIndexBodyModel( config, ngCollection ) ) 
			};

			var fileContents = template.GenerateString();

			File.WriteAllText( 
				Path.Combine( config.BaseDir, "index.html" ), 
				fileContents, 
				Encoding.UTF8 );
		}

		static void GenerateDecadeIndexes( IProjectConfig config, NGCollection ngCollection ) {
			Func<IDecade,NamedLink> createLink = decade => {
				if( decade == null )
					return null;

				return new NamedLink( decade.DisplayName, decade.IndexFileName );
			};

			foreach( var decadeContext in ngCollection.GetAllDecades( hydrate:false ).WithContext() ) {
				var template = new SiteLayout() {
					Model = new SiteLayoutModel(
						config: config,
						pageTitle: "NatGeo: The " + decadeContext.Current.DisplayName,
						depth: 1,
						bodyModel: new DecadeBodyModel( config: config,
							issues: ngCollection.GetAllIssuesInDecade( decadeContext.Current ),
							previous: createLink( decadeContext.Previous ),
							next: createLink( decadeContext.Next ) ) )
				};

				var fileContents = template.GenerateString();

				File.WriteAllText(
					Path.Combine( config.AbsoluteHtmlDir, decadeContext.Current.IndexFileName ),
					fileContents,
					Encoding.UTF8 );
			}
		}

		static void GenerateYearIndexes( IProjectConfig config, NGCollection ngCollection ) {
			Func<int,string> yearToDecade = year => (year / 10) + "x";

			var yearContexts = 
				ngCollection.GetAllIssues().
				GroupBy( i => i.ReleaseDate.Year,
					(key, issues ) => new{Year = key, Issues = issues.ToArray() } ).
				WithContext();

			foreach( var yearContext in yearContexts ) {
				NamedLink previous = null;
				if( yearContext.Previous != null ) {
					var url = yearContext.Previous.Year + ".html";

					if( yearToDecade( yearContext.Previous.Year ) != yearToDecade( yearContext.Current.Year ) ) {
						url = UriPath.CombineWithDepth( 1, yearToDecade( yearContext.Previous.Year ), url );
					}

					previous = new NamedLink( 
						name: yearContext.Previous.Year.ToString(),
						url: url );
				}

				NamedLink next = null;
				if( yearContext.Next != null ) {
					var url = yearContext.Next.Year + ".html";

					if( yearToDecade( yearContext.Next.Year ) != yearToDecade( yearContext.Current.Year ) ) {
						url = UriPath.CombineWithDepth( 1, yearToDecade( yearContext.Next.Year ), url );
					}

					next = new NamedLink( 
						name: yearContext.Next.Year.ToString(),
						url: url );
				}


				var template = new SiteLayout() {
					Model = new SiteLayoutModel(
						config: config,
						pageTitle: "NatGeo: " + yearContext.Current.Year,
						depth: 2,
						bodyModel: new YearBodyModel( 
							config: config,
							year: yearContext.Current.Year,
							issues: yearContext.Current.Issues,
							previous: previous,
							next: next ) )
				};

				var fileContents = template.GenerateString();

				var path = Path.Combine( config.AbsoluteHtmlDir,
					           yearToDecade( yearContext.Current.Year ),
					           yearContext.Current.Year + ".html" );
				Utility.CreatePath( path );
				File.WriteAllText(
					path,
					fileContents,
					Encoding.UTF8 );
			}
		}

		static void GenerateIssueIndexes( IProjectConfig config, NGCollection ngCollection ) {
			Func<IIssue,IIssue, NamedLink> createLink = (issue, current ) => {
				if( issue == null )
					return null;

				var url = UriPath.Combine( issue.DirectoryName, issue.IndexFileName );

				if( issue.DecadeId != current.DecadeId ) {
					url = UriPath.CombineWithDepth( 2, issue.Decade.DirectoryName, url );
				} else {
					url = UriPath.CombineWithDepth( 1, url );
				}

				return new NamedLink( issue.LongDisplayName, url );
			};

			foreach( var issueContext in ngCollection.GetAllIssues( hydrateCoverPage:false ).WithContext() ) {
				var template = new SiteLayout() {
					Model = new SiteLayoutModel(
						config: config,
						pageTitle: "NatGeo: " + issueContext.Current.LongDisplayName,
						depth: 3,
						bodyModel: new IssueBodyModel( config: config,
							issue: issueContext.Current,
							pages: ngCollection.GetAllPagesInIssue( issueContext.Current ),
							previous: createLink( issueContext.Previous, issueContext.Current ),
							next: createLink( issueContext.Next, issueContext.Current ) ) )
				};

				var fileContents = template.GenerateString();

				var path = Path.Combine( 
					           config.AbsoluteHtmlDir, 
					           issueContext.Current.Decade.DirectoryName, 
					           issueContext.Current.DirectoryName, 
					           issueContext.Current.IndexFileName );

				Utility.CreatePath( path );

				File.WriteAllText(
					path,
					fileContents,
					Encoding.UTF8 );
			}
		}

		static void GeneratePageIndexes( IProjectConfig config, NGCollection ngCollection ) {
			Func<IPage, NamedLink> createLink = page => {
				if( page == null )
					return null;

				return new NamedLink( page.DisplayName, page.IndexName );
			};

			foreach( var issue in ngCollection.GetAllIssues( hydrateCoverPage:false ) ) {
				var pages = ngCollection.GetAllPagesInIssue( issue ).ToArray();
				foreach( var pageContext in pages.WithContext() ) {
					var template = new SiteLayout() {
						Model = new SiteLayoutModel(
							config: config,
							pageTitle: String.Format( "NatGeo: {0} of {1}", pageContext.Current.DisplayName, pages.Length ),
							depth: 3,
							bodyModel: new PageBodyModel( config: config,
								page: pageContext.Current,
								previous: createLink( pageContext.Previous ),
								next: createLink( pageContext.Next ) ) )
					};

					var fileContents = template.GenerateString();

					var path = Path.Combine( 
						           config.AbsoluteHtmlDir, 
						           pageContext.Current.Issue.Decade.DirectoryName, 
						           pageContext.Current.Issue.DirectoryName, 
						           pageContext.Current.IndexName );

					Utility.CreatePath( path );

					File.WriteAllText(
						path,
						fileContents,
						Encoding.UTF8 );
				}
			}
		}

		static string GetThumbnailHtml( string displayName,
		                                string previewText,
		                                string imgUrl,
		                                string linkUrl,
		                                int width,
		                                int height ) {
			return String.Format( 
				@"<div class=""col-md-3 col-sm-3 col-sx-3"">
							<a href=""{3}"">
								<div class=""panel panel-default"">
									<div class=""panel-heading"">
										<h3 class=""panel-title"">{0}</h3>
									</div>
									<div class=""panel-body"">
										<img src=""{2}"" width=""{4}"" height=""{5}"" class=""img-thumbnail center-block"" alt=""{1}""/>
									</div>
								</div>
							</a>
						</div>", displayName, previewText, imgUrl, linkUrl, width, height );
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
		/*
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

					foreach( var batch in issue.InBatchesOf(4) ) {
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
		*/
		/*
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
		*/
		static NamedLink MakeLinkToPage( NGPage page ) {
			if( page == null )
				return null;
			return new NamedLink( name: page.DisplayName, url: page.IndexName );
		}
	}
}
