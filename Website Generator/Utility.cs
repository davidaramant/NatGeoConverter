using System.IO;

namespace Website_Generator {
	public static class Utility {
		public static void CreateDirInFilePath( string path ) {
			var dirPart = Path.GetDirectoryName( path );
			CreateDir( dirPart );
		}

		public static void CreateDir( string path )	{
			if( !Directory.Exists( path ) ) {
				Directory.CreateDirectory( path );
			}
		}
	}
}
