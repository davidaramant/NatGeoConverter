using System;
using System.IO;

namespace Utilities {
	public static class UriPath {
		public static string Combine( params string[] components )
		{
			var unixPath = Path.DirectorySeparatorChar == '/';

			var url = Path.Combine( components );

			if( !unixPath ) {
				url = url.Replace( Path.DirectorySeparatorChar, '/' );
			}

			return url;
		}
	}
}

