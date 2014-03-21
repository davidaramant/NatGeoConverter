using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Utilities.PathExtensions;

namespace DataModel {
    public sealed class NGIssue : IEnumerable<NGPage> {
        readonly List<NGPage> _pages = new List<NGPage>();

		public int Count { get { return _pages.Count; } }
		public NGPage this[int index] {
			get { return _pages[ index ]; }
		}

        readonly DateTime _releaseDate;

        public DateTime ReleaseDate { get { return _releaseDate; } }

        public NGPage Cover {
			get 
			{ 
				try
				{
					return _pages.First( page => Regex.IsMatch( Path.GetFileName( page.FullPath ), @"^NGM_(\w{2}_)?\d{4}") );
				}
				catch {
					Console.Out.WriteLine( "Error with {0}", _releaseDate.ToString("s") );
					throw;
				}
			}
        }

		public string ShortDisplayName {
			get { return _releaseDate.ToString( "MMM d" ); }
        }

		public string LongDisplayName {
			get { return _releaseDate.ToString( "MMMM d, yyyy" ); }
		}


		public string DirectoryName { get { return _releaseDate.ToString( "yyyyMMdd" ); } }

		public string RelativeIndexUrl {
			get { return Path.Combine( DirectoryName, IndexFileName ); }
		}

		public string IndexFileName { 
			get { return "index.html"; }
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
				Directory.GetFiles( path, searchPattern: "*.jpg" ).OrderBy( _ => _ ).Select( (pagePath,index) => NGPage.Parse( pagePath, basePath: basePath, index:index ) ),
                    releaseDate: releaseDate );
        }

        public IEnumerator<NGPage> GetEnumerator() {
            return _pages.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
