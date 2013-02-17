using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DataModel {
    [DebuggerDisplay("{ToString()}")]
    public sealed class NGDecade : IEnumerable<NGIssue> {
        readonly List<NGIssue> _issues = new List<NGIssue>();
        readonly string _name;

        public string Name { get { return _name; } }

        public NGDecade( IEnumerable<NGIssue> issues, string name ) {
            _issues.AddRange( issues.OrderBy( _ => _.ReleaseDate ) );
            _name = name;
        }

        public static NGDecade Parse( string path, string basePath ) {
            return new NGDecade(
                    Directory.GetDirectories( path ).Select( issueDir => NGIssue.Parse( issueDir, basePath: basePath ) ),
                    name: path.Substring( path.LastIndexOf( Path.DirectorySeparatorChar ) + 1 ) );
        }

        public IEnumerator<NGIssue> GetEnumerator() {
            return _issues.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public override string ToString() {
            return String.Format( "{0} {1} issues", _name, _issues.Count() );
        }

        public string Serialize() {
            return "decade;" + _name;
        }
    }
}
