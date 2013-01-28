using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DataModel {
    public sealed class NGIssue : IEnumerable<NGPage> {
        readonly List<NGPage> _normalPages = new List<NGPage>();
        readonly List<NGPage> _specialPages = new List<NGPage>();

        public NGIssue( IEnumerable<NGPage> pages, IEnumerable<NGPage> specialPages ) {
            _normalPages.AddRange( pages );
            _specialPages.AddRange( specialPages );
        }

        public static NGIssue Parse( string path ) {
            var allPages = Directory.GetFiles( path ).Select( pagePath => new NGPage( pagePath ) );

            return null;
        }

        public IEnumerator<NGPage> GetEnumerator() {
            return _normalPages.Concat( _specialPages ).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
