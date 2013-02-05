using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace DataModel {
    [DebuggerDisplay( "{ToString()}" )]
    public sealed class NGPage {
        private string _path;

        public int Year { get; private set; }
        public int Month { get; private set; }
        public string IssueQualifier { get; private set; }
        public int PageNumber { get; private set; }

        public bool IsSpecial { get; private set; }
        public bool Failed { get; private set; }

        public string FullPath { get { return _path; } }

        public NGPage( string path ) {
            _path = path;

            var fileName = Path.GetFileName( path );

            var match = Regex.Match( fileName, @"NGM_(\d{4})_(\d{2})(\w)?_(\d{3})_4.jpg", RegexOptions.Compiled );

            if( match.Success ) {
                Year = Int32.Parse( match.Groups[1].Value );
                Month = Int32.Parse( match.Groups[2].Value );
                IssueQualifier = match.Groups[3].Success ? match.Groups[3].Value : String.Empty;
                PageNumber = Int32.Parse( match.Groups[4].Value );
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
            return _path;
        }
    }
}
