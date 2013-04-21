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

            //"NGM_SM_1893_04_4.jpg" -> "04"
            //"NGM_1893_04A_001_4.jpg" -> "04A_001"
            //"NGM_SW_1993_11_001_4.jpg" -> "11_001"

            if( !isSpecial ) {
                fileName = Regex.Replace( fileName, @"^NGM_(\w{2}_)?(\d{4})_", "" );
                fileName = Regex.Replace( fileName, @"_4.jpg$", "" );

                int pageNumber = 0;
                if( fileName.Contains( "_" ) ) {
                    var match = Regex.Match( fileName, @"\d{2}\w?_(\d{3})", RegexOptions.Compiled );
                    pageNumber = Int32.Parse( match.Groups[1].Value );
                } else {
                    pageNumber = Int32.Parse( fileName );
                }

                return new NGPage(
                            fullPath: fullPath,
                            relativePath: relativePath,
                            pageNumber: pageNumber,
                            displayName: pageNumber.ToString(),
                            isSpecial: false );
            } else {
                //"1930_01_Florida.jpg"
                //"1899_10_NORTH CAROLINA TENNESSEE.jpg"
                var stuffToRemove = new[] {
                            @"^\d{4}_\d{2}_",
                            @".jpg$",
                        };

                var fixedName = fileName;

                foreach( var r in stuffToRemove ) {
                    fixedName = Regex.Replace( fixedName, r, "" );
                }

                return new NGPage(
                            fullPath: fullPath,
                            relativePath: relativePath,
                            pageNumber: null,
                            displayName: fixedName.Replace( '_', ' ' ).TrimStart(),
                            isSpecial: true );
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
