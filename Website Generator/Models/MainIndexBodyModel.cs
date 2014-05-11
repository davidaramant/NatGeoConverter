using System;
using System.Collections.Generic;
using DataModel;
using Utilities;

namespace Website_Generator.Models {
	public sealed class MainIndexBodyModel : BaseBodyModel, IBodyModel {
		private readonly NGCollection _ngCollection;

		public IEnumerable<Decade> GetDecades() {
			return _ngCollection.GetAllDecades();
		}

		public MainIndexBodyModel( IProjectConfig config, NGCollection ngCollection )  : 
		base( config: config,
			previous: null,
			next: null,
			allowResize: false ) {
			_ngCollection = ngCollection;
		}

		public string GetBody() {
			var template = new MainIndexBody() { Model = this };
			return template.GenerateString();
		}

		public IEnumerable<NamedLink> GetBreadcrumbParts() {
			yield return NamedLink.Empty( "Decades" );
		}
	}
}

