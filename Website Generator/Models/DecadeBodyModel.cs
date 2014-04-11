using System;
using System.Collections.Generic;
using Utilities;
using DataModel;
using System.Linq;

namespace Website_Generator.Models {
	public sealed class DecadeBodyModel : BaseBodyModel, IBodyModel {
		readonly IDecade _decade;
		readonly IEnumerable<IIssue> _issues;

		public string GetBody() {
			var template = new DecadeBody() { Model = this };
			return template.GenerateString();
		}

		public IDecade Decade { get { return _decade; } }

		public IEnumerable<IIssue> GetIssues() {
			return _issues;
		}

 	    public IEnumerable<NamedLink> GetBreadcrumbParts() {
			yield return new NamedLink( "Decades", UriPath.CombineWithDepth( 1, "index.html" ) );
			yield return NamedLink.Empty( _decade.DisplayName );
		}

		public DecadeBodyModel( 
			IProjectConfig config, 
			IEnumerable<IIssue> issues, 
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

