using System;
using System.IO;
using SQLite;

namespace DataModel {
	public sealed class Article {
		#region Database

		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[Indexed]
		public int IssueId { get; set; }

		[Indexed]
		public int PageId { get; set; }

        public int Length { get; set; }

		public string Description { get; set; }

		public string Summary { get; set; }

		#endregion Database
	}
}

