using System;
using System.IO;
using SQLite;

namespace DataModel.Database {
	public sealed class Decade : IDecade {
		#region Database

		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[MaxLength( 4 )]
		public string DirectoryName { get; set; }

		[Indexed]
		public int PreviewPageId { get; set; }

		#endregion Database

		[Ignore]
		public string DisplayName {
			get { return DirectoryName.Replace( "x", "0s" ); }
		}

		[Ignore]
		public string IndexFileName {
			get { return DirectoryName + ".html"; }
		}

		[Ignore]
		public IPage PreviewPage { get; set; }
	}
}

