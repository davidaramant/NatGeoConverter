using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DataModel {

    public sealed class NGDecade : IEnumerable<NGIssue> {
        readonly List<NGIssue> _issues = new List<NGIssue>();

        public NGDecade( IEnumerable<NGIssue> issues ) {
            _issues.AddRange( issues );
        }

        public static NGDecade Parse( string path ) {
            return new NGDecade( Directory.GetDirectories( path ).Select( NGIssue.Parse ) );
        }

        public IEnumerator<NGIssue> GetEnumerator() {
            return _issues.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
