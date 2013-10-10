using System;
using System.IO;

namespace DataModel.Extensions {
	public static class PathExtensions {
		public static string GetPathRelativeTo( this string fullPath, string basePath )
		{
			return Path.GetFullPath( fullPath ).
				Replace( Path.GetFullPath( basePath ), String.Empty ).
					TrimStart( Path.DirectorySeparatorChar );
		}
	}
}

