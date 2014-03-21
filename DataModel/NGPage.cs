using System;
using System.Diagnostics;
using System.IO;
using Utilities.PathExtensions;

namespace DataModel {
    [DebuggerDisplay( "{ToString()}" )]
    public sealed class NGPage {
		private readonly string _fullPath;
		private readonly string _relativePath;

		private readonly string _displayName;

		public string FullPath { get { return _fullPath; } }
		public string RelativePath { get { return _relativePath; } }
		public string DisplayName { get { return _displayName; } }

		public string NormalThumbnailFullPath { get { return FullPath.Replace( "full", "thumbs"); } }
		public string RetinaThumbnailFullPath { get { return NormalThumbnailFullPath.Replace(".jpg","@2x.jpg"); } }

		public string NormalThumbnailUrl { get { return _relativePath.Replace("full","thumbs"); } }

		public string IndexName { get { return Path.GetFileNameWithoutExtension( _relativePath ) + ".html"; } }

		public NGPage( string basePath, string fullPath, int index ) {
			_fullPath = fullPath;
			_relativePath = fullPath.GetPathRelativeTo( basePath );

			_displayName = "Page " + (index + 1);
        }

		public static NGPage Parse( string fullPath, string basePath, int index ) {
			return new NGPage( fullPath: fullPath, basePath: basePath, index:index );
        }

        public override string ToString() {
            return RelativePath;
        }
    }
}
