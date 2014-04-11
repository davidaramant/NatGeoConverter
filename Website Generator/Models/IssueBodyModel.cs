using System;
using System.Collections.Generic;
using Utilities;
using DataModel;
using System.Linq;

namespace Website_Generator.Models {
	public sealed class IssueBodyModel: BaseBodyModel, IBodyModel {
		readonly IIssue _issue;
		readonly IEnumerable<IPage> _pages;

		public string GetBody() {
			var template = new IssueBody() { Model = this };
			return template.GenerateString();
		}

		public IDecade Decade { get { return _issue.Decade; } }

		public IIssue Issue { get { return _issue; } }

		public IEnumerable<IPage> GetPages() {
			return _pages;
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
		                       IIssue issue,
		                       IEnumerable<IPage> pages,
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

