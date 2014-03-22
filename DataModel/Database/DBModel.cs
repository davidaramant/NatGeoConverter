using System;
using SQLite;

namespace DataModel.Database {
	public sealed class Decade {
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		[MaxLength(4)]
		public string DirectoryName { get; set;}
	}

	public sealed class Issue {
		[PrimaryKey, AutoIncrement]
		public int Id {get;set;}
		public DateTime ReleaseDate { get; set;}
		[Indexed]
		public int DecadeId {get;set;}
	}

	public sealed class Page {
		[PrimaryKey, AutoIncrement]
		public int Id {get;set;}
		[MaxLength(54)] // This is the longest file name
		public string FileName { get; set;}
		[Indexed]
		public int IssueId { get; set; }
		[Indexed]
		public int DecadeId { get; set; }
		public int FullImageWidth { get; set; }
		public int FullImageHeight { get; set; }
		public int ThumbnailImageWidth { get; set; }
		public int ThumbnailImageHeight { get; set; }
	}
}