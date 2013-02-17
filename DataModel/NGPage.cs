using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace DataModel {
    [DebuggerDisplay( "{ToString()}" )]
    public sealed class NGPage {
        private readonly string _path;
        private readonly string _relativePath;

        public int Year { get; private set; }
        public int Month { get; private set; }
        public string IssueQualifier { get; private set; }
        public int PageNumber { get; private set; }

        public string DisplayName {
            get {
                if( IsSpecial ) {
                    var stuffToRemove = new [] {
                        @"NGM_\d{4}_\d{2}_",
                        @"\d{4}_\d{2}_",
                        @"NGM_SM_",
                        @"_4.jpg",
                    };

                    var fixedName = _relativePath;

                    foreach( var r in stuffToRemove ) {
                        fixedName = Regex.Replace( fixedName, r, "" );
                    }

                    return Path.GetFileNameWithoutExtension( fixedName.Replace('_', ' ' ).TrimStart() );
                } else {
                    return PageNumber.ToString();
                }
            }
        }

        public bool IsSpecial { get; private set; }
        public bool Failed { get; private set; }

        public string FullPath { get { return _path; } }

        public string RelativePath { get { return _relativePath; } }

        public NGPage( string path, string basePath ) {
            _path = path;

            _relativePath = 
                Path.GetFullPath( path ).
                Replace( Path.GetFullPath( basePath ), String.Empty ).
                TrimStart( Path.DirectorySeparatorChar );

            var fileName = Path.GetFileName( path );

            var match = Regex.Match( fileName, @"NGM_(\w{2}_)?(\d{4})_(\d{2})(\w)?_(\d{2}_)?(\d{3})_4.jpg", RegexOptions.Compiled );

            if( match.Success ) {
                Year = Int32.Parse( match.Groups[2].Value );
                Month = Int32.Parse( match.Groups[3].Value );
                IssueQualifier = match.Groups[4].Success ? match.Groups[4].Value : String.Empty;
                PageNumber = Int32.Parse( match.Groups[6].Value );
            } else {
                IsSpecial = true;

                var specialMatch = Regex.Match( fileName, @"(\d{4})_(\d{2})(\w)?_.+.jpg", RegexOptions.Compiled );
                if( specialMatch.Success ) {
                    Year = Int32.Parse( specialMatch.Groups[1].Value );
                    Month = Int32.Parse( specialMatch.Groups[2].Value );
                    IssueQualifier = specialMatch.Groups[3].Success ? specialMatch.Groups[3].Value : String.Empty;
                } else {
                    Failed = true;
                }
            }

        }

        public override string ToString() {
            return _relativePath;
        }

        public string Serialize() {
            return "page;" + _relativePath;
        }
    }
}
