using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using DataModel.Database;
using SQLite;
using Utilities.PathExtensions;
using Utilities;
using System.Drawing;
using System.Text.RegularExpressions;
using DataModelLoader.NGModel.Extensions;

namespace DataModelLoader {
	class MainClass {
		public static void Main( string[] args ) {
			var config = new ProjectConfig( baseDir: Path.Combine( "/", "Users", "davidaramant", "Web", "NatGeo" ) );

			var timer = System.Diagnostics.Stopwatch.StartNew();

			LoadNGIssues( config );

			return;

			PopulateDatabase( config );

			Console.Out.WriteLine( "Generating model took: {0}", timer.Elapsed );

			CheckDatabase( dbPath: config.DatabasePath );
		}

		private static void LoadNGIssues( IProjectConfig config ) {
			var exceptions = new List<NGModel.Extensions.PageExceptions>();

			var dbPath = Path.Combine( config.DatabaseDir, "cngcontent.sqlite3" );
			using( var db = new SQLiteConnection( dbPath ) ) {
				var issues = db.Table<NGModel.issues>().ToArray();

				foreach( var issue in issues ) {
					try{
					exceptions.Add(	issue.GetPageExceptions() );
					}
					catch( Exception e ) {
						Out.WL( "{0}: {1}", issue.search_time, e );
					}
				}

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

						var allPages = 
							Directory.GetFiles( issueDir, "*.jpg", SearchOption.TopDirectoryOnly ).
							OrderBy( name => name ).
							Select( (fullImagePath, index ) => {
								var fileName = Path.GetFileName( fullImagePath );
								var fullSize = ImageSizeLoader.GetJpegImageSize( fullImagePath );
								var thumbSize = ImageSizeLoader.GetJpegImageSize( GetThumbnailPath( config,
									                decadeName,
									                issueDirName,
									                fileName ) );
								return new Page { 
									IssueId = issue.Id,
									Number = index + 1,
									FileName = fileName,
									FullImageWidth = fullSize.Width,
									FullImageHeight = fullSize.Height,
									ThumbnailImageWidth = thumbSize.Width,
									ThumbnailImageHeight = thumbSize.Height,
								}; 
							} ).ToList();

						db.InsertAll( allPages );

						issue.CoverPageId = allPages.First( page => Regex.IsMatch( page.IndexName, @"^NGM_(\w{2}_)?\d{4}" ) ).Id;
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
