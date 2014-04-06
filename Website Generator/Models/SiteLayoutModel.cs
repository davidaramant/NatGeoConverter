using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace Website_Generator.Models {
	public sealed class SiteLayoutModel : BaseModel {
		readonly int _depth;
		readonly IBodyModel _bodyModel;

		public string PageTitle{ get; private set; }

		public string IconUrl { 
			get { return UriPath.CombineWithDepth( _depth, "favicon_v04.ico" ); } 
		}

		public IEnumerable<string> GetCssUrls() {
			return new [] {
				"bootstrap.min.css",
				"customizations.css"
			}.Select( name => UriPath.CombineWithDepth( _depth, "css", name ) );
		}

		public IEnumerable<string> GetJSUrls() {
			var javascriptFiles = new List<string> {
				"jquery.min.js",
				"bootstrap.min.js",
			};
			// TODO: Add optional JS files to BodyModel
			if( false ) {
				javascriptFiles.Add( "imageFitToggles.js" );
			}

			return javascriptFiles.Select( name => UriPath.CombineWithDepth( _depth, "js", name ) );
		}

		public IBodyModel Body { get { return _bodyModel; } }

		public SiteLayoutModel( IProjectConfig config, string pageTitle, int depth, IBodyModel bodyModel ) : base(config) {
			PageTitle = pageTitle;
			_depth = depth;
			_bodyModel = bodyModel;
		}

		public Action<System.IO.TextWriter> RenderBody() {
			return RenderUnescapedHtml( _bodyModel.GetBody() );
		}
	}
}

