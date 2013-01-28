using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DataModel {
    public sealed class NGIssue : IEnumerable<NGPage> {
        readonly List<NGPage> _pages = new List<NGPage>();
        readonly List<NGPage> _specialPages = new List<NGPage>();

        bool HasSpecialPages {
            get { return _specialPages.Any(); }
        }

        public NGIssue( IEnumerable<NGPage> pages ) {
            _pages.AddRange( pages.Where( p => !p.IsSpecial ) );
            _specialPages.AddRange( pages.Where( p => p.IsSpecial ) );
        }

        public static NGIssue Parse( string path ) {
            return new NGIssue( Directory.GetFiles( path, searchPattern: "*.jpg" ).Select( pagePath => new NGPage( pagePath ) ) );
        }

        public IEnumerator<NGPage> GetEnumerator() {
            return _pages.Concat( _specialPages ).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
