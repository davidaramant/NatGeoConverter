using System;

namespace Website_Generator {
	public interface IBodyModel {
		/// <summary>
		/// Gets the body contents as a string.
		/// </summary>
		/// <returns>The body contents.</returns>
		string GetBody();
	}
}