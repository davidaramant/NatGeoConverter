using System;
using System.Collections.Generic;
using Utilities;
using DataModel;
using System.Linq;

namespace Website_Generator.Models {
	public sealed class DecadeBodyModel : BaseBodyModel, IBodyModel {
		readonly Decade _decade;
		readonly IEnumerable<Issue> _issues;

		public string GetBody() {
			var template = new DecadeBody() { Model = this };
			return template.GenerateString();
		}

		public Decade Decade { get { return _decade; } }

		public IEnumerable<Issue> GetIssues() {
			return _issues;
		}

 	    public IEnumerable<NamedLink> GetBreadcrumbParts() {
			yield return new NamedLink( "Decades", UriPath.CombineWithDepth( 1, "index.html" ) );
			yield return NamedLink.Empty( _decade.DisplayName );
		}

		public DecadeBodyModel( 
			IProjectConfig config, 
			IEnumerable<Issue> issues, 
			NamedLink previous, 
			NamedLink next ) : 
			base( config: config,
			     previous: previous,
			     next: next,
			     allowResize: false ) {
			_issues = issues;
			_decade = issues.First().Decade;
		}
	}
}

