using System;
using System.Drawing;

namespace Utilities {
	public interface IProjectConfig {
		string DatabasePath { get; }
		string BaseDir { get; }

		string AbsoluteFullImageDir { get; }
		string AbsoluteThumbnailImageDir { get; }
		string AbsoluteHtmlDir { get; }

		string RelativeFullImageDir { get;}
		string RelativeThumbnailImageDir { get; }
		string RelativeHtmlDir { get; }

		Size ThumbnailSize { get; }
	}
}