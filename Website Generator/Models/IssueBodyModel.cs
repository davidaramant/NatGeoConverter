using System;
using System.Collections.Generic;
using Utilities;
using DataModel;
using System.Linq;

namespace Website_Generator.Models {
	public sealed class IssueBodyModel: BaseBodyModel, IBodyModel {
		readonly Issue _issue;
		readonly IEnumerable<Page> _pages;

		public string GetBody() {
			var template = new IssueBody() { Model = this };
			return template.GenerateString();
		}

		public Decade Decade { get { return _issue.Decade; } }

		public Issue Issue { get { return _issue; } }

		public Page Cover{
			get { return _pages.First(); }
		}

		public string CoverThumbnailUrl {
			get{ return UriPath.CombineWithDepth( 3,
					Config.RelativeThumbnailImageDir,
					Decade.DirectoryName,
					Issue.DirectoryName,
					Cover.FileName ); }
		}

		public IEnumerable<Page> GetPages() {
			return _pages.Skip(1);
		}

		public IEnumerable<NamedLink> GetBreadcrumbParts() {
			yield return new NamedLink( "Decades", UriPath.CombineWithDepth( 3, "index.html" ) );
			yield return new NamedLink( Decade.DisplayName, UriPath.CombineWithDepth( 2, Decade.IndexFileName ) );
			yield return new NamedLink( Issue.ReleaseDate.Year.ToString(),
				UriPath.CombineWithDepth( 1,
					Issue.ReleaseDate.Year + ".html" ) );
			yield return NamedLink.Empty( Issue.ShortDisplayName );
		}

		public IssueBodyModel( IProjectConfig config,
		                       Issue issue,
		                       IEnumerable<Page> pages,
		                       NamedLink previous,
		                       NamedLink next ) : base( config: config,
		                                                   previous: previous,
		                                                   next: next,
		                                                   allowResize: false ) {
			_pages = pages;
			_issue = issue;
		}
	}
}

