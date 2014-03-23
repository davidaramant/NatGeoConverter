using System;

namespace Utilities {
	public static class Out {
		public static void WL() {
			Console.Out.WriteLine();
		}

		public static void WL( string format, params object[] args ) {
			Console.Out.WriteLine( format, args );
		}
	}
}

