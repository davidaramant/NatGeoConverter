using System;
using System.IO;
using Utilities;

namespace Website_Generator {
	public abstract class BaseModel {
		public IProjectConfig Config { get; private set; }

		// Gross hack due to what the Xamarin generated model for a Razor template looks like when not using ASP.NET
		protected Action<TextWriter> RenderUnescapedHtml( string html ) {
			return new Action<TextWriter>( writer => writer.Write( html ) );
		}			

		public Action<TextWriter> RenderThumbnail(
			string linkUrl,
			string description,
			string imgUrl,
			int imgWidth,
			int imgHeight,
			string imgAltText)
		{
			var template = new Thumbnail() { 
				Model = new ThumbnailModel(
						config: Config,
						linkUrl:linkUrl, 
						description:description, 
						imgUrl:imgUrl, 
						imgWidth:imgWidth,
						imgHeight:imgHeight,
						imgAltText:imgAltText ) 
			};

			return RenderUnescapedHtml( template.GenerateString() );
		}

		protected BaseModel( IProjectConfig config )
		{
			Config = config;
		}
	}
}