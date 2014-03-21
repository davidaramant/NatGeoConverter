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
			get { return Path.Combine( BaseDir, "imgs", "full" ); }
		}

		public string BaseThumbnailImageDir {
			get { return Path.Combine( BaseDir, "imgs", "thumbs" ); }
		}

		public string BaseHtmlDir {
			get { return Path.Combine( BaseDir, "html" ); }
		}

		public ProjectConfig( string baseDir ) {
			_baseDir = baseDir;
		}
	}
}

