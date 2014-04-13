using System.Collections.Generic;
using Utilities;
using System.IO;

namespace Website_Generator.OtherContent {
	public static class Content {
		public static class CSS {
			public const string Bootstrap = "bootstrap.min.css";
			public const string Customizations = "customizations.css";

			public static IEnumerable<string> GetAll() {
				yield return Bootstrap;
				yield return Customizations;
			}
		}

		public static class JS {
			public const string Bootstrap = "bootstrap.min.js";
			public const string JQuery = "jquery.min.js";
			public const string ImageFit = "imageFitToggles.js";

			public static IEnumerable<string> GetAll() {
				yield return Bootstrap;
				yield return JQuery;
				yield return ImageFit;
			}
		}

		public static readonly IEnumerable<string> Fonts = new [] {
			"glyphicons-halflings-regular.eot",
			"glyphicons-halflings-regular.svg",
			"glyphicons-halflings-regular.ttf",
			"glyphicons-halflings-regular.woff",
		};

		public static void CopyToOutput( IProjectConfig config ) {
			var baseSourcePath = "OtherContent";

			Utility.CreateDir( Path.Combine( config.BaseDir, "css" ) );
			Utility.CreateDir( Path.Combine( config.BaseDir, "js" ) );
			Utility.CreateDir( Path.Combine( config.BaseDir, "fonts" ) );

			foreach( var cssFile in CSS.GetAll() ) {
				File.Copy( 
					Path.Combine( baseSourcePath, "css", cssFile ),
					Path.Combine( config.BaseDir, "css", cssFile ), 
					overwrite:true );
			}

			foreach( var jsFile in JS.GetAll() ) {
				File.Copy( 
					Path.Combine( baseSourcePath, "js", jsFile ),
					Path.Combine( config.BaseDir, "js", jsFile ), 
					overwrite:true );
			}

			foreach( var fontFile in Fonts ) {
				File.Copy( 
					Path.Combine( baseSourcePath, "fonts", fontFile ),
					Path.Combine( config.BaseDir, "fonts", fontFile ), 
					overwrite:true );
			}
		}
	}
}

