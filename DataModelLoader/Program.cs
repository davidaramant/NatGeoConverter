using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DataModel;
using SQLite;
using Utilities;
using Utilities.EnumerableExtensions;
using Utilities.PathExtensions;
using DataModelLoader.NGModel.Extensions;

namespace DataModelLoader {
	class MainClass {
		public static void Main( string[] args ) {
			var config = new ProjectConfig( baseDir: Path.Combine( "/", "Users", "davidaramant", "Web", "NatGeo" ) );

			//LoadNGIssues(config);
			//return;

			var timer = System.Diagnostics.Stopwatch.StartNew();

			//PopulateDatabase( config );
			ReDoPageOrder( config );

			Console.Out.WriteLine( "Generating model took: {0}", timer.Elapsed );

			CheckDatabase( dbPath: config.DatabasePath );
		}

		private static void LoadNGIssues( IProjectConfig config ) {
			var dbPath = Path.Combine( config.DatabaseDir, "cngcontent.sqlite3" );

			using( var db = new SQLiteConnection( dbPath ) ) {
				var issues = 
					db.Table<NGModel.issues>().Where( i => i.search_time < 20090000 ).ToArray().
					Select( i => new { Issue = i, Exceptions = i.GetPageExceptions() } ).
					ToArray();
	
				var allLargePages = issues.SelectMany( i => i.Exceptions.large_pages );

				var pageCountGroups = allLargePages.GroupBy( lp => lp.page_count );

				foreach( var g in pageCountGroups ) {
					Out.WL( "Page count: {0} - Number# {1}", g.First().page_count, g.Count() );
					if( g.First().page_count == 18 ) {
						var lp = g.First();
					}
				}

				return;


				foreach( var op in Enum.GetValues( typeof( CorrectionOperation)).Cast<CorrectionOperation>() ) {
					var issuesWithOp = 
						issues.Where( i => i.Exceptions.corrections.Any( c => c.GetOperation() == op ) ).
						OrderBy( i => i.Issue.search_time ).ToArray();
					Out.WL( new string( '#', 120 ) );
					Out.WL( new string( '#', 120 ) );
					Out.WL( new string( '#', 120 ) );
					Out.WL( "{0}: {1}", op, issuesWithOp.Length );

					foreach( var i in issuesWithOp ) {
						Out.WL( "Issue: {0}", i.Issue.search_time );
						Out.WL( "E: {0}", JsonUtility.FormatJson( NGModelExtensions.ConvertToJson( i.Issue.page_exceptions ) ) );
						Out.WL();
					}
				}
			}
		}

		private static NGModel.issues LoadNGIssue( IProjectConfig config, Issue issue ) {
			var dbPath = Path.Combine( config.DatabaseDir, "cngcontent.sqlite3" );

			var searchTime = issue.GetSearchTime();
			using( var db = new SQLiteConnection( dbPath ) ) {
				return db.Table<NGModel.issues>().Where( i => i.search_time == searchTime ).First();
			}
		}

		private static void CheckDatabase( string dbPath ) {
			using( var db = new SQLiteConnection( dbPath ) ) {
				Console.Out.WriteLine( "Decades: {0}\tIssues: {1}\tPages: {2}",
					db.Table<Decade>().Count(),
					db.Table<Issue>().Count(),
					db.Table<Page>().Count() );
			}
		}

		private static void ReDoPageOrder( IProjectConfig config ) {
			using( var db = new SQLiteConnection( databasePath: config.DatabasePath ) ) {
				foreach( var issue in db.Table<Issue>().ToArray() ) {
					var ngIssue = LoadNGIssue( config, issue );

					var pagesInIssue = db.Table<Page>().Where( p => p.IssueId == issue.Id ).ToArray();

					var orderedPages = OrderPages( ngIssue, pagesInIssue ).ToList();

					db.UpdateAll( orderedPages );
				}
			}
		}

		private static void PopulateDatabase( IProjectConfig config ) {
			using( var db = new SQLiteConnection( databasePath: config.DatabasePath ) ) {
				db.DropTable<Decade>();
				db.DropTable<Issue>();
				db.DropTable<Page>();

				db.CreateTable<Decade>();
				db.CreateTable<Issue>();
				db.CreateTable<Page>();

				foreach( var decadeDir in Directory.GetDirectories( config.AbsoluteFullImageDir ) ) {
					var decadeName = decadeDir.GetLastDirectory();
					var decade = new Decade { DirectoryName = decadeName };
					db.Insert( decade );
					Console.Out.WriteLine( "Decade: {0}", decadeName );

					bool firstIssue = true;

					foreach( var issueDir in Directory.GetDirectories( decadeDir ) ) {
						var issueDirName = issueDir.GetLastDirectory();
						var issue = new Issue { DecadeId = decade.Id, ReleaseDate = ParseIssueDirIntoDate( issueDirName ) };
						db.Insert( issue );

						var ngIssue = LoadNGIssue( config, issue );

						var allPages = 
							Directory.GetFiles( issueDir, "*.jpg", SearchOption.TopDirectoryOnly ).
							Select( (fullImagePath, index ) => {
								var fileName = Path.GetFileName( fullImagePath );
								var fullSize = ImageSizeLoader.GetJpegImageSize( fullImagePath );
								var thumbSize = ImageSizeLoader.GetJpegImageSize( GetThumbnailPath( config,
									                decadeName,
									                issueDirName,
									                fileName ) );
								return new Page { 
									IssueId = issue.Id,
									FileName = fileName,
									FullImageWidth = fullSize.Width,
									FullImageHeight = fullSize.Height,
									ThumbnailImageWidth = thumbSize.Width,
									ThumbnailImageHeight = thumbSize.Height,
								}; 
							} ).ToList();

						var orderedPages = OrderPages( ngIssue, allPages ).ToList();

						db.InsertAll( orderedPages );

						issue.CoverPageId = orderedPages[0].Id;
						db.Update( issue );

						if( firstIssue ) {
							firstIssue = false;
							decade.PreviewPageId = issue.CoverPageId;
							db.Update( decade );
						}
					}
				}
			}
		}


		private static IEnumerable<Page> OrderPages( NGModel.issues ngIssue, IEnumerable<Page> unorderdPages ) {
			var exceptions = ngIssue.GetPageExceptions();
			var normalPages = 
				unorderdPages.
				Where( p => p.FileName.StartsWith( exceptions.basename ) ).
				OrderBy( p => p.FileName ).
				ToList();
			var supplements = unorderdPages.Except( normalPages ).OrderBy( p => p.FileName );

			// Handle exceptions
			foreach( var correction in exceptions.corrections ) {
				switch( correction.GetOperation() ) {
					case CorrectionOperation.MoveImage:
						var indexOfPage = normalPages.FindIndex( p => p.FileName == correction.filename );
						normalPages.Move( indexOfPage, correction.adjustment.Value );
						break;				
					case CorrectionOperation.UnnumberedImage:
						var page = normalPages.First( p => p.FileName == correction.filename );
						page.Unnumbered = true;
						break;
					default:
						break;
				}
			}

			var largePageMap = exceptions.large_pages.ToDictionary( lp => lp.filename );

			// Set numbers
			int currentPageNumber = ngIssue.numbered_page_start_value;
			foreach( var page in normalPages.Take( ngIssue.numbered_page_offset ) ) {
				page.Unnumbered = true;
			}
			foreach( var page in normalPages.Skip( ngIssue.numbered_page_offset ) ) {
				if( !page.Unnumbered ) {
					page.Number = currentPageNumber++;

					LargePage lp = null;
					if( largePageMap.TryGetValue( page.FileName, out lp ) ) {
						currentPageNumber += lp.page_count-1;
					}
				}
			}

			// Set order
			var orderedPages = new List<Page>();
			orderedPages.AddRange( normalPages );
			foreach( var supplement in supplements ) {
				supplement.Unnumbered = true;
				orderedPages.Add( supplement );
			}


			return orderedPages.Select( (p, index ) => {
				var order = index;
				p.Order = order;
				return p;
			} );
		}

		private static string GetThumbnailPath( IProjectConfig config,
		                                        string decadeDir,
		                                        string issueDir,
		                                        string imgFileName ) {
			return Path.Combine( config.AbsoluteThumbnailImageDir,
				decadeDir,
				issueDir,
				Path.GetFileName( imgFileName ) );
		}

		private static DateTime ParseIssueDirIntoDate( string issueDirName ) {
			DateTime releaseDate;
			var success = DateTime.TryParseExact( issueDirName,
				              "yyyyMMdd",
				              System.Globalization.CultureInfo.InvariantCulture,
				              System.Globalization.DateTimeStyles.None,
				              out releaseDate );

			if( !success ) {
				throw new ArgumentException( "Unknown date format for one of the issues: " + issueDirName );
			}
			return releaseDate;
		}
	}
}
