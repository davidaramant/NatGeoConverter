using System;
using System.Diagnostics;
using System.IO;
using DataModel.Extensions;

namespace DataModel {
    [DebuggerDisplay( "{ToString()}" )]
    public sealed class NGPage {
		private readonly string _fullPath;
		private readonly string _relativePath;

		public string FullPath { get { return _fullPath; } }
		public string RelativePath { get { return _relativePath; } }
		public string DisplayName { get { return Path.GetFileNameWithoutExtension(RelativePath); } }

		public string NormalThumbnailFullPath { get { return FullPath.Replace( "full", "thumbs"); } }
		public string RetinaThumbnailFullPath { get { return NormalThumbnailFullPath.Replace(".jpg","@2x.jpg"); } }

		public string NormalThumbnailUrl { get { return _relativePath.Replace("full","thumbs"); } }

		public string IndexName { get { return Path.GetFileNameWithoutExtension( _relativePath ) + ".html"; } }

		public NGPage( string basePath, string fullPath ) {
			_fullPath = fullPath;
			_relativePath = fullPath.GetPathRelativeTo( basePath );
        }

        public static NGPage Parse( string fullPath, string basePath ) {
			return new NGPage( fullPath: fullPath, basePath: basePath );
        }

        public override string ToString() {
            return RelativePath;
        }
    }
}
