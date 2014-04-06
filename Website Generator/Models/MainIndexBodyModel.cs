using System;
using System.Collections.Generic;
using DataModel;
using Utilities;

namespace Website_Generator.Models {
	public sealed class MainIndexBodyModel : BaseModel, IBodyModel {
		private readonly NGCollection _ngCollection;

		public IEnumerable<IDecade> GetDecades() {
			return _ngCollection.GetAllDecades();
		}

		public MainIndexBodyModel( IProjectConfig config, NGCollection ngCollection ) : base( config ) {
			_ngCollection = ngCollection;
		}

		public string GetBody() {
			var template = new MainIndexBody() { Model = this };
			return template.GenerateString();
		}

		public string BodyClass { get { return null; } }

		public IEnumerable<NamedLink> GetBreadcrumbParts() {
			yield return NamedLink.Empty( "Decades" );
		}

		public NamedLink Previous { get { return null; } }

		public NamedLink Next { get { return null; } }

		public bool AllowResize { get { return false; } }

		public string AllowResizeText { get { return AllowResize ? (string)null : "disabled"; } }
	}
}

