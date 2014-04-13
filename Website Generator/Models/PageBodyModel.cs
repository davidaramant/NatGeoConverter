﻿using System;
using System.Collections.Generic;
using Utilities;
using DataModel;
using System.Linq;

namespace Website_Generator.Models {
	public sealed class PageBodyModel : BaseBodyModel, IBodyModel {
		readonly IPage _page;
		readonly int _totalPages;

		public string GetBody() {
			var template = new PageBody() { Model = this };
			return template.GenerateString();
		}

		public string ImageUrl {
			get {
				return UriPath.CombineWithDepth( 3,
					Config.RelativeFullImageDir,
					Decade.DirectoryName,
					Issue.DirectoryName,
					Page.FileName );
			}
		}

		public IDecade Decade { get { return _page.Issue.Decade; } }

		public IIssue Issue { get { return _page.Issue; } }

		public IPage Page { get { return _page; } }

		public IEnumerable<NamedLink> GetBreadcrumbParts() {
			yield return new NamedLink( "Decades", UriPath.CombineWithDepth( 3, "index.html" ) );
			yield return new NamedLink( Decade.DisplayName, UriPath.CombineWithDepth( 2, Decade.IndexFileName ) );
			yield return new NamedLink( Issue.ReleaseDate.Year.ToString(),
				UriPath.CombineWithDepth( 1,
					Issue.ReleaseDate.Year + ".html" ) );
			yield return new NamedLink( Issue.ShortDisplayName, Issue.IndexFileName );
			yield return NamedLink.Empty( String.Format( "{0} of {1}", Page.DisplayName, _totalPages ) );
		}

		public override IEnumerable<string> GetExtraJSFiles() {
			yield return "imageFitToggles.js";
		}

		protected override string GetBodyClass() {
			return "page-body";
		}

		public PageBodyModel( IProjectConfig config,
		                      IPage page,
		                      int totalPages,
		                      NamedLink previous,
		                      NamedLink next ) : base( 
			config: config, 
			previous: previous, 
			next: next, 
			allowResize: true ) {
			_page = page;
			_totalPages = totalPages;
		}
	}
}
