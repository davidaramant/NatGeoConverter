using System;
using System.Collections.Generic;
using DataModel;
using Utilities;

namespace Website_Generator {
	public sealed class MainIndexBodyModel : BaseModel {
		public IEnumerable<IDecade> GetDecades() {
			yield break;
		}

		public MainIndexBodyModel( IProjectConfig config ) : base(config) {
		}
	}
}

