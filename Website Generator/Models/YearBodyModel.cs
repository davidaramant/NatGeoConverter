using System;
using System.Collections.Generic;
using Utilities;
using DataModel;
using System.Linq;

namespace Website_Generator.Models {
	public sealed class YearBodyModel: BaseModel, IBodyModel {
		readonly int _year;
		readonly IDecade _decade;
		readonly IEnumerable<IIssue> _issues;

		public string GetBody() {
			var template = new YearBody() { Model = this };
			return template.GenerateString();
		}

		public int Year { get { return _year; } }

		public IDecade Decade { get { return _decade; } }

		public IEnumerable<IIssue> GetIssues() {
			return _issues;
		}

		public string BodyClass { get { return null; } }

		public IEnumerable<NamedLink> GetBreadcrumbParts() {
			yield return new NamedLink( "Decades", UriPath.CombineWithDepth( 2, "index.html" ) );
			yield return new NamedLink( _decade.DisplayName, UriPath.CombineWithDepth( 1, _decade.IndexFileName ) );
			yield return NamedLink.Empty( Year.ToString() );
		}

		public NamedLink Previous { get; private set; }

		public NamedLink Next { get; private set; }

		public bool AllowResize { get { return false; } }

		public string AllowResizeText { get { return AllowResize ? (string)null : "disabled"; } }

		public YearBodyModel( IProjectConfig config,
		                      int year,
		                      IEnumerable<IIssue> issues,
		                      NamedLink previous,
		                      NamedLink next ) : base( config ) {
			_year = year;
			_issues = issues;
			_decade = issues.First().Decade;
			Previous = previous;
			Next = next;
		}
	}
}

