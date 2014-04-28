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

	public sealed class Page : IPage {
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
		public IIssue Issue { get; set; }
	}
}