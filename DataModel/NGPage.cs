using System;
using System.Diagnostics;
using System.IO;
using DataModel.Extensions;

namespace DataModel {
    [DebuggerDisplay( "{ToString()}" )]
    public sealed class NGPage {
        public readonly string FullPath;
        public readonly string RelativePath;
		public readonly string DisplayName;

        public NGPage( string fullPath, string relativePath, string displayName ) {
            FullPath = fullPath;
            RelativePath = relativePath;
			DisplayName = displayName;
        }

        public static NGPage Parse( string fullPath, string basePath ) {
			var relativePath = fullPath.GetPathRelativeTo( basePath );

			var displayName = Path.GetFileNameWithoutExtension( relativePath );

			return new NGPage( fullPath: fullPath, relativePath: relativePath, displayName:displayName );
        }

        public override string ToString() {
            return RelativePath;
        }

        public string Serialize() {
            return "page;" + RelativePath;
        }
    }
}
