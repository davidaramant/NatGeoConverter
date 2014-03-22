using System;

namespace Utilities {
	public interface IProjectConfig {
		string DatabasePath { get; }
		string BaseDir { get; }

		string BaseFullImageDir { get; }
		string BaseThumbnailImageDir { get; }
		string BaseHtmlDir { get; }

		string RelativeFullImageDir { get;}
		string RelativeThumbnailImageDir { get; }
		string RelativeHtmlDir { get; }
	}
}