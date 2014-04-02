using System;

namespace Utilities {
	public sealed class NamedLink {
		private readonly string _url;
		private readonly string _name;

		public string Name { get { return _name; } }

		public string Url { get { return _url; } }

		public bool HasUrl { get { return !String.IsNullOrWhiteSpace( Url ); } }

		public NamedLink( string name, string url ) {
			_name = name;
			_url = url;
		}

		public static NamedLink Empty( string name ) {
			return new NamedLink( name: name, url: null );
		}
	}
}

