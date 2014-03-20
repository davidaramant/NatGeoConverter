using System;
using System.IO;
using System.Linq;

namespace Utilities.Extensions {
	public static class PathExtensions {
		public static string GetPathRelativeTo( this string fullPath, string basePath )
		{
			return Path.GetFullPath( fullPath ).
				Replace( Path.GetFullPath( basePath ), String.Empty ).
					TrimStart( Path.DirectorySeparatorChar );
		}

		public static string GetLastDirectory( this string fullPath ) {
			var cleanedPath = fullPath.Last() == Path.DirectorySeparatorChar ? fullPath.Substring(0,fullPath.Length - 1) : fullPath;
			return cleanedPath.Substring( cleanedPath.LastIndexOf( Path.DirectorySeparatorChar) + 1 );

		}
	}
}

