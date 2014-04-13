using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;
using Website_Generator.OtherContent;

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
				Content.CSS.Bootstrap,
				Content.CSS.Customizations
			}.Select( name => UriPath.CombineWithDepth( _depth, "css", name ) );
		}

		public IEnumerable<string> GetJSUrls() {
			var javascriptFiles = new [] {
				Content.JS.JQuery,
				Content.JS.Bootstrap,
			}.Concat( _bodyModel.GetExtraJSFiles() );

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

