using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace Website_Generator {
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

		public string BodyClass { get { return null; } }

		public IEnumerable<NamedLink> GetBreadcrumbParts() {
			throw new NotImplementedException( "Figure out how to do breadcrumbs" );
		}

		public NamedLink Previous { get { throw new NotImplementedException("Pass in Previous"); } }

		public NamedLink Next { get { throw new NotImplementedException("Pass in Next"); } }

		public bool AllowResize { get { throw new NotImplementedException("Pass in AllowResize"); } }

		public string AllowResizeText { get { return AllowResize ? (string)null : "disabled"; } }

		public IEnumerable<string> GetJSUrls() {
			var javascriptFiles = new List<string> {
				"jquery.min.js",
				"bootstrap.min.js",
			};
			throw new NotImplementedException( "Add optional JS files to BodyModel" );
			if( false ) {
				javascriptFiles.Add( "imageFitToggles.js" );
			}

			return javascriptFiles.Select( name => UriPath.CombineWithDepth( _depth, "js", name ) );
		}

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

