using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DataModel.Extensions;

namespace DataModel {
    [DebuggerDisplay( "{ToString()}" )]
    public sealed class NGIssue : IEnumerable<NGPage> {
        readonly List<NGPage> _pages = new List<NGPage>();

        readonly DateTime _releaseDate;

        public DateTime ReleaseDate { get { return _releaseDate; } }

        public NGPage Cover {
            get { return _pages[0]; }
        }

        public string DisplayName {
            get { return _releaseDate.ToString( "yyyy - MM - dd" ); }
        }

        public string Name {
            get { return _releaseDate.ToString( "yyyy-MM-dd" ); }
        }

        public NGIssue( IEnumerable<NGPage> pages, DateTime releaseDate ) {
            _pages.AddRange( pages.OrderBy( _ => _.RelativePath ) );
            _releaseDate = releaseDate;
        }

        public static NGIssue Parse( string path, string basePath ) {
			DateTime releaseDate;
			var input = path.GetLastDirectory();

			var success = DateTime.TryParseExact( input,
				"yyyyMMdd",
				System.Globalization.CultureInfo.InvariantCulture,
				System.Globalization.DateTimeStyles.None,
				out releaseDate );

			if( !success ) {
				throw new ArgumentException( "Unknown date format for one of the issues: " + input );
			}		

            return new NGIssue(
                    Directory.GetFiles( path, searchPattern: "*.jpg" ).OrderBy( _ => _ ).Select( pagePath => NGPage.Parse( pagePath, basePath: basePath ) ),
                    releaseDate: releaseDate );
        }

        public IEnumerator<NGPage> GetEnumerator() {
            return _pages.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public string Serialize() {
            return "issue;" + _releaseDate.ToString( "s" );
        }

        public override string ToString() {
            return String.Format( "{0} {1} pages", DisplayName, _pages.Count() );
        }
    }
}
