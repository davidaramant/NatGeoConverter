using System;
using System.IO;
using SQLite;

namespace DataModel.Database {
	public sealed class Issue : IIssue {
		#region Database

		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public DateTime ReleaseDate { get; set; }

		[Indexed]
		public int DecadeId { get; set; }

		[Indexed]
		public int CoverPageId { get; set; }

		#endregion Database

		[Ignore]
		public IDecade Decade { get; set; }

		[Ignore]
		public string ShortDisplayName {
			get { return ReleaseDate.ToString( "MMM d" ); }
		}

		[Ignore]
		public string LongDisplayName {
			get { return ReleaseDate.ToString( "MMMM d, yyyy" ); }
		}

		[Ignore]
		public string IndexFileName {
			get { return DirectoryName + ".html"; }
		}

		[Ignore]
		public string DirectoryName {
			get { return ReleaseDate.ToString( "yyyyMMdd" ); }
		}

		[Ignore]
		public IPage CoverPage { get; set; }
	}

}

