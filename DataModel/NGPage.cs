using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace DataModel {
    [DebuggerDisplay( "{ToString()}" )]
    public sealed class NGPage {
        public readonly string FullPath;
        public readonly string RelativePath;
        public readonly int? PageNumber;
        public readonly string DisplayName;
        public readonly bool IsSpecial;

        public NGPage( string fullPath, string relativePath, int? pageNumber, string displayName, bool isSpecial ) {
            FullPath = fullPath;
            RelativePath = relativePath;
            PageNumber = pageNumber;
            DisplayName = displayName;
            IsSpecial = isSpecial;
        }

        public static NGPage Parse( string fullPath, string basePath ) {

            var relativePath =
                Path.GetFullPath( fullPath ).
                Replace( Path.GetFullPath( basePath ), String.Empty ).
                TrimStart( Path.DirectorySeparatorChar );

            var fileName = Path.GetFileName( fullPath );

            var isSpecial = !fileName.StartsWith( "NGM_" );

            if( !isSpecial ) {
                var match = Regex.Match( fileName, @"NGM_(\w{2}_)?(\d{4})_(\d{2})(\w)?_(\d{2}_)?(\d{3})_4.jpg", RegexOptions.Compiled );

                if( match.Success ) {
                    var year = Int32.Parse( match.Groups[2].Value );
                    var month = Int32.Parse( match.Groups[3].Value );
                    var issueQuaifier = match.Groups[4].Success ? match.Groups[4].Value : String.Empty;
                    var pageNumber = Int32.Parse( match.Groups[6].Value );
                    return new NGPage( 
                                fullPath: fullPath, 
                                relativePath: relativePath, 
                                pageNumber: pageNumber, 
                                displayName: pageNumber.ToString(), 
                                isSpecial: false );
                } else {
                    throw new Exception( "failed" );
                }
            } else {
                var stuffToRemove = new[] {
                            @"NGM_\d{4}_\d{2}_",
                            @"\d{4}_\d{2}_",
                            @"NGM_SM_",
                            @"_4.jpg",
                        };

                var fixedName = relativePath;

                foreach( var r in stuffToRemove ) {
                    fixedName = Regex.Replace( fixedName, r, "" );
                }

                return new NGPage(
                            fullPath: fullPath,
                            relativePath: relativePath,
                            pageNumber: null,
                            displayName: Path.GetFileNameWithoutExtension( fixedName.Replace( '_', ' ' ).TrimStart() ),
                            isSpecial: false );
            }
        }

        public override string ToString() {
            return RelativePath;
        }

        public string Serialize() {
            return "page;" + RelativePath;
        }
    }
}
