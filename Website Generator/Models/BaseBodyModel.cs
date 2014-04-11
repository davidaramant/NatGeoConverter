using System;
using Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Website_Generator.Models {
	public abstract class BaseBodyModel : BaseModel {
		public NamedLink Previous { get; private set; }

		public NamedLink Next { get; private set; }

		public bool AllowResize { get; private set; }

		public string AllowResizeText { get { return AllowResize ? (string)null : "disabled"; } }

		public string BodyClass { get { return GetBodyClass(); } }

		protected BaseBodyModel( IProjectConfig config, NamedLink previous, NamedLink next, bool allowResize ) : base( config ) {
			Previous = previous;
			Next = next;
			AllowResize = allowResize;
		}

		public virtual IEnumerable<string> GetExtraJSFiles()
		{
			return Enumerable.Empty<string>();
		}

		protected virtual string GetBodyClass()
		{
			return null;
		}
	}
}

