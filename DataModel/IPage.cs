using System.Drawing;

namespace DataModel {
	public interface IPage {
		int Number { get; }
		string DisplayName { get; }
		string IndexName { get; }

		string RelativeFullUrl { get; }
		string RelativeThumbnailUrl { get; }

		Size FullSize { get; }
		Size ThumbnailDisplaySize { get; }	
	}
}