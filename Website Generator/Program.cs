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

			OtherContent.Content.CopyToOutput( config );

			//string newPages = @"/Users/davidaramant/Desktop/Fixed Pages";




			//return; 

			var model = new NGCollection( config );
			var timer = Stopwatch.StartNew();

			GenerateMainIndex( config, model );
			GenerateDecadeIndexes( config, model );
			GenerateYearIndexes( config, model );
			GenerateIssueIndexes( config, model );
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

			var path = Path.Combine( config.BaseDir, "index.html" );
			SaveFile( path, tw => template.Generate( tw ) );
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

				var path = Path.Combine( config.AbsoluteHtmlDir, decadeContext.Current.IndexFileName );

				SaveFile( path, tw => template.Generate( tw ) );
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

				var path = Path.Combine( config.AbsoluteHtmlDir,
					           yearToDecade( yearContext.Current.Year ),
					           yearContext.Current.Year + ".html" );

				SaveFile( path, tw => template.Generate( tw ) );
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

				var path = Path.Combine( 
					           config.AbsoluteHtmlDir, 
					           issueContext.Current.Decade.DirectoryName, 
					           issueContext.Current.DirectoryName, 
					           issueContext.Current.IndexFileName );

				SaveFile( path, tw => template.Generate( tw ) );
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
							pageTitle: String.Format( "NatGeo: {0}", pageContext.Current.DisplayName ),
							depth: 3,
							bodyModel: new PageBodyModel( config: config,
								page: pageContext.Current,
								totalPages: pages.Length,
								previous: createLink( pageContext.Previous ),
								next: createLink( pageContext.Next ) ) )
					};

					var path = Path.Combine( 
						           config.AbsoluteHtmlDir, 
						           pageContext.Current.Issue.Decade.DirectoryName, 
						           pageContext.Current.Issue.DirectoryName, 
						           pageContext.Current.IndexName );

					SaveFile( path, tw => template.Generate( tw ) );
				}
			}
		}

		static void SaveFile( string filePath, Action<TextWriter> contentWriter ) {
			Utility.CreateDirInFilePath( filePath );

			using( var stream = new StreamWriter( filePath, false, Encoding.UTF8 ) ) {
				contentWriter( stream );
			}
		}
	}
}
