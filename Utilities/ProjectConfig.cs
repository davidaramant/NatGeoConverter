using System.IO;
using System.Drawing;

namespace Utilities {
	public class ProjectConfig : IProjectConfig {
		private readonly string _baseDir;

		public string DatabasePath {
			get { return Path.Combine( DatabaseDir, "imgDatabase.sqlite3" ); }
		}

		public string DatabaseDir {
			get { return Path.Combine( BaseDir, "data" ); }
		}

		public string BaseDir {
			get { return _baseDir; }
		}

		public string AbsoluteFullImageDir {
			get { return Path.Combine( BaseDir, RelativeFullImageDir ); }
		}

		public string AbsoluteThumbnailImageDir {
			get { return Path.Combine( BaseDir, RelativeThumbnailImageDir ); }
		}

		public string AbsoluteHtmlDir {
			get { return Path.Combine( BaseDir, RelativeHtmlDir ); }
		}

		public string RelativeHtmlDir { get { return "html"; } }

		public string RelativeFullImageDir { get { return Path.Combine( "imgs", "full" ); } }

		public string RelativeThumbnailImageDir  { get { return Path.Combine( "imgs", "thumbs" ); } }

		public Size ThumbnailSize { get { return new Size( 360, 520 ); } } 

		public ProjectConfig( string baseDir ) {
			_baseDir = baseDir;
		}
	}
}

