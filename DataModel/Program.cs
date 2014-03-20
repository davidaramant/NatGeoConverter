using System;
using SQLite;

namespace DataModel {
	public static class Program {
		static void Main( string[] args ) {
			using( var db = new SQLiteConnection( databasePath: "foofoo.sqlite" ) ) {
				db.CreateTable<Person>();
				db.Insert( new Person { FirstName = "Test", LastName = "Dude" + DateTime.Now.Second } );
			}

			using( var db = new SQLiteConnection( databasePath: "foofoo.sqlite" ) ) {
				foreach( var p in db.Table<Person>() ) {
					Console.Out.WriteLine( "{0} {1}", p.FirstName, p.LastName );
				}
				Console.Out.WriteLine( "Path : " + db.DatabasePath );
			}
		}

		public class Person
		{
			[PrimaryKey, AutoIncrement]
			public int ID { get; set; }
			public string FirstName { get; set; }
			public string LastName { get; set; }
		}
	}
}

