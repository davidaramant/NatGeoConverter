using System;

namespace Website_Generator {
	public class ThumbnailModel {
		public string LinkUrl { get; private set; }

		public string Description { get; private set; }

		public string ImgUrl { get; private set; }

		public int ImgWidth { get; private set; }

		public int ImgHeight { get; private set; }

		public string ImgAltText { get; private set; }

		public ThumbnailModel(
			string linkUrl,
			string description,
			string imgUrl,
			int imgWidth,
			int imgHeight,
			string imgAltText ) {
			LinkUrl = linkUrl;
			Description = description;
			ImgUrl = imgUrl;
			ImgWidth = imgWidth;
			ImgHeight = imgHeight;
			ImgAltText = imgAltText;
		}
	}
}