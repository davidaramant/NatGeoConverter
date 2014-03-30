using System;
using System.IO;
using System.Linq;

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

		public static string CombineWithDepth( int depth, params string[] components )
		{
			return Combine( Enumerable.Repeat("..",depth).Concat( components ).ToArray() );
		}
	}
}

