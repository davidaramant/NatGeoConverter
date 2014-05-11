using System;
using System.IO;
using SQLite;

namespace DataModel {
	public sealed class Page {
		#region Database

		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public int Order { get; set; }

		public int Number { get; set; }

		public bool Unnumbered { get; set; }

		[MaxLength( 54 )] // This is the longest file name
		public string FileName { get; set; }

		[Indexed]
		public int IssueId { get; set; }

		public int FullImageWidth { get; set; }

		public int FullImageHeight { get; set; }

		public int ThumbnailImageWidth { get; set; }

		public int ThumbnailImageHeight { get; set; }

		#endregion Database

		[Ignore]
		public string DisplayName {
			get 
			{ 
				var display = Order.ToString();
				if( !Unnumbered ) {
					display += " (Page " + Number + ")";
				}
				return display;
			}
		}

		[Ignore]
		public string IndexName {
			get { return Path.GetFileNameWithoutExtension( FileName ) + ".html"; }
		}

		[Ignore]
		public int ThumbnailImageDisplayWidth {
			get { return ThumbnailImageWidth / 2; }
		}

		[Ignore]
		public int ThumbnailImageDisplayHeight {
			get { return ThumbnailImageHeight / 2; }
		}

		[Ignore]
		public string DecadeDirName { get; set; }

		[Ignore]
		public string IssueDirName { get; set; }

		[Ignore]
		public Issue Issue { get; set; }
	}
}

