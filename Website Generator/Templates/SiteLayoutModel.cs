using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;
using System.Web;
using System.Web.Mvc;

namespace Website_Generator {
	public class SiteLayoutModel {
		readonly int _depth;

		public string PageTitle{ get; private set; }

		public string IconUrl 
		{ 
			get { return UriPath.CombineWithDepth(_depth,"favicon_v04.ico"); } 
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
			if( false ) {
				javascriptFiles.Add( "imageFitToggles.js" );
			}

			return javascriptFiles.Select( name => UriPath.CombineWithDepth( _depth, "js", name ) );
		}

		public SiteLayoutModel( string pageTitle, int depth ) {
			PageTitle = pageTitle;
			_depth = depth;
		}

		public Action<System.IO.TextWriter> RenderBody()
		{
			return RenderUnescapedHtml( @"<h1>Hello World</h1>" );
		}

		// Gross hack due to what the Xamarin generated model for a Razor template looks like when not using ASP.NET
		protected Action<System.IO.TextWriter> RenderUnescapedHtml( string html )
		{
			return new Action<System.IO.TextWriter>( writer => writer.Write( html ));
		}
	}
}

