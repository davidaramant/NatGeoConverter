using System;
using System.Diagnostics;
using System.IO;
using Utilities.PathExtensions;
using System.Drawing;
using Utilities;

namespace DataModel {
	[DebuggerDisplay( "{ToString()}" )]
	public sealed class NGPage {
		private readonly string _fileName;
		private readonly int _number;
		private readonly string _displayName;
		private readonly string _relativeFullUrl;
		private readonly string _relativeThumbnailUrl;
		private readonly Size _fullSize;
		private readonly Size _thumbnailSize;

		public int Number { get { return _number; } }

		public string DisplayName { get { return _displayName; } }

		public string IndexName { get { return Path.GetFileNameWithoutExtension( _fileName ) + ".html"; } }

		public string RelativeFullUrl {
			get { return _relativeFullUrl; }
		}

		public string RelativeThumbnailUrl {
			get { return _relativeThumbnailUrl; }
		}

		public Size FullSize {
			get { return _fullSize; }
		}

		public Size ThumbnailDisplaySize {
			get { return _thumbnailSize; }
		}

		public NGPage( 
			IProjectConfig config, 
			string decadeDir,
			string issueDir, 
			string fileName, 
			int pageNumber,
			Size fullSize,
			Size thumbnailSize ) {
			_fileName = fileName;
			_number = pageNumber;
			_displayName = "Page " + pageNumber;
			_relativeFullUrl = Path.Combine( config.RelativeFullImageDir, decadeDir, issueDir, fileName );
			_relativeThumbnailUrl = Path.Combine( config.RelativeThumbnailImageDir, decadeDir, issueDir, fileName );
			_fullSize = fullSize;
			_thumbnailSize = thumbnailSize;

		}

		public override string ToString() {
			return RelativeFullUrl;
		}
	}
}
