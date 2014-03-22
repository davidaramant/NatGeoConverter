using System.IO;

namespace Utilities {
	public class ProjectConfig : IProjectConfig {
		private readonly string _baseDir;

		public string DatabasePath {
			get { return Path.Combine( BaseDir, "data", "imgDatabase.sqlite" ); }
		}

		public string BaseDir {
			get { return _baseDir; }
		}

		public string BaseFullImageDir {
			get { return Path.Combine( BaseDir, RelativeFullImageDir ); }
		}

		public string BaseThumbnailImageDir {
			get { return Path.Combine( BaseDir, RelativeThumbnailImageDir ); }
		}

		public string BaseHtmlDir {
			get { return Path.Combine( BaseDir, RelativeHtmlDir ); }
		}

		public string RelativeHtmlDir { get { return "html"; } }

		public string RelativeFullImageDir { get { return Path.Combine( "imgs", "full" ); } }

		public string RelativeThumbnailImageDir  { get { return Path.Combine( "imgs", "thumbs" ); } }

		public ProjectConfig( string baseDir ) {
			_baseDir = baseDir;
		}
	}
}

