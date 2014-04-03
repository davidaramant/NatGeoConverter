using System;
using System.Collections.Generic;
using DataModel;
using Utilities;

namespace Website_Generator {
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
	}
}

