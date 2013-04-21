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

            try {
                var relativePath =
                    Path.GetFullPath( fullPath ).
                    Replace( Path.GetFullPath( basePath ), String.Empty ).
                    TrimStart( Path.DirectorySeparatorChar );

                var fileName = Path.GetFileName( fullPath );

                var isSpecial = !fileName.StartsWith( "NGM_" );

                // PROBLEM: Some pages have same page number but are A, B, C, D, etc
                // PROBLEM: Need custom sort collection

                //"NGM_SM_1893_04_4.jpg" -> "04"
                //"NGM_1893_04A_001_4.jpg" -> "04A_001"
                //"NGM_SW_1993_11_001_4.jpg" -> "11_001"
                //"NGM_1892_05_051_801_4.jpg" -> "051_801"
                //"NGM_1892_05_a4.jpg" -> "a4"

                if( !isSpecial ) {
                    fileName = Regex.Replace( fileName, @"^NGM_(\w{2}_)?(\d{4})_", "" );
                    fileName = Regex.Replace( fileName, @"_4.jpg$", "" );

                    int? pageNumber = null;
                    if( Regex.IsMatch( fileName, @"\d{2}\w?_(\d{3})" ) ) {
                        var match = Regex.Match( fileName, @"\d{2}\w?_(\d{3})", RegexOptions.Compiled );
                        pageNumber = Int32.Parse( match.Groups[1].Value );
                    } else {
                        int temp = 0;
                        var isNumber = Int32.TryParse( fileName, out temp );

                        if( !isNumber ) {
                            isSpecial = true;
                        } else {
                            pageNumber = temp;
                        }
                    }

                    return new NGPage(
                                fullPath: fullPath,
                                relativePath: relativePath,
                                pageNumber: pageNumber,
                                displayName: isSpecial ? fileName : pageNumber.ToString(),
                                isSpecial: isSpecial );
                } else {
                    //"1930_01_Florida.jpg"
                    //"1899_10_NORTH CAROLINA TENNESSEE.jpg"
                    var stuffToRemove = new[] {
                            @"^\d{4}_\d{2}_",
                            @"(_4)?.jpg$",
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
            } catch {
                Console.Out.WriteLine( "Fucked page: " + fullPath );
                throw;
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
