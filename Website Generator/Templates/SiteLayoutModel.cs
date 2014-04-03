using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace Website_Generator {
	public sealed class SiteLayoutModel : BaseModel {
		readonly int _depth;

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
			yield return new NamedLink( "Link", "http://google.com" );
			yield return NamedLink.Empty( "Decades" );
		}

		public NamedLink Previous { get { return new NamedLink("Nowhere", "http://google.com"); } }

		public NamedLink Next { get { return new NamedLink("Nowhere", "http://google.com"); } }

		public bool AllowResize { get { return false; } }

		public string AllowResizeText { get { return AllowResize ? (string)null : "disabled"; } }

		public IEnumerable<string> GetJSUrls() {
			var javascriptFiles = new List<string> {
				"jquery.min.js",
				"bootstrap.min.js",
			};
			if( false ) {
				javascriptFiles.Add( "imageFitToggles.js" );
			}

			return javascriptFiles.Select( name => UriPath.CombineWithDepth( _depth, "js", name ) );
		}

		public SiteLayoutModel( IProjectConfig config, string pageTitle, int depth ) : base(config) {
			PageTitle = pageTitle;
			_depth = depth;
		}

		public Action<System.IO.TextWriter> RenderBody() {
			return RenderUnescapedHtml( @"<h1>Hello World</h1>" );
		}
	}
}

