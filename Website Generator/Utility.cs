using System.IO;

namespace Website_Generator {
	public static class Utility {
		public static void CreatePath( string path ) {
			var dirPart = Path.GetDirectoryName( path );
			if( !Directory.Exists( dirPart ) ) {
				Directory.CreateDirectory( dirPart );
			}
		}
	}
}
