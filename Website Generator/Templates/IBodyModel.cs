using System;
using System.Collections.Generic;
using DataModel;
using Utilities;

namespace Website_Generator {
	public interface IBodyModel {
		/// <summary>
		/// Gets the body contents as a string.
		/// </summary>
		/// <returns>The body contents.</returns>
		string GetBody();

		string BodyClass{ get; }

		IEnumerable<NamedLink> GetBreadcrumbParts();

		NamedLink Previous { get; }

		NamedLink Next { get; }

		bool AllowResize { get; }

		string AllowResizeText { get; }
	}
}