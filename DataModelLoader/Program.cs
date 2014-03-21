using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using DataModel.Database;
using SQLite;
using Utilities.PathExtensions;
using Utilities;

namespace DataModelLoader {
	class MainClass {
		public static void Main( string[] args ) {
			var config = new ProjectConfig( baseDir: Path.Combine( "/", "Users", "davidaramant", "Web", "NatGeo" ) );

			var timer = System.Diagnostics.Stopwatch.StartNew();

			PopulateDatabase( baseImgPath: config.BaseFullImageDir, dbPath: config.DatabasePath );

			Console.Out.WriteLine( "Generating model took: {0}", timer.Elapsed );
			timer.Reset();

			CheckDatabase( dbPath: config.DatabasePath );

			Console.Out.WriteLine( "Accessing DB took: {0}", timer.Elapsed );
		}

		private static void CheckDatabase( string dbPath ) {
			using( var db = new SQLiteConnection( dbPath ) ) {
				Console.Out.WriteLine( "Decades: {0}\tIssues: {1}\tPages: {2}",
					db.Table<Decade>().Count(),
					db.Table<Issue>().Count(),
					db.Table<Page>().Count() );
			}
		}

		private static void PopulateDatabase( string baseImgPath, string dbPath ) {
			using( var db = new SQLiteConnection( databasePath: dbPath ) ) {
				db.DropTable<Decade>();
				db.DropTable<Issue>();
				db.DropTable<Page>();

				db.CreateTable<Decade>();
				db.CreateTable<Issue>();
				db.CreateTable<Page>();

				foreach( var decadeDir in Directory.GetDirectories( baseImgPath ).OrderBy( name => name ) ) {
					var decadeId = db.Insert( new Decade { DirectoryName = decadeDir.GetLastDirectory() } );
					Console.Out.WriteLine( "Decade: {0}", decadeDir.GetLastDirectory() );

					foreach( var issueDir in Directory.GetDirectories( decadeDir ).OrderBy( name => name ) ) {
						var issueId = db.Insert( 
							new Issue { DecadeId = decadeId, ReleaseDate = ParseIssueDirIntoDate( issueDir ) } );

						var allPages = 
							Directory.GetFiles( issueDir ).
							OrderBy( name => name ).
							Select( name => new Page { IssueId = issueId, FileName = Path.GetFileName( name ) } );

						db.InsertAll( allPages );
					}
				}
			}
		}

		private static DateTime ParseIssueDirIntoDate( string fullIssuePath ) {
			DateTime releaseDate;
			var success = DateTime.TryParseExact( fullIssuePath.GetLastDirectory(),
				              "yyyyMMdd",
				              System.Globalization.CultureInfo.InvariantCulture,
				              System.Globalization.DateTimeStyles.None,
				              out releaseDate );

			if( !success ) {
				throw new ArgumentException( "Unknown date format for one of the issues: " + fullIssuePath );
			}
			return releaseDate;
		}
	}
}
