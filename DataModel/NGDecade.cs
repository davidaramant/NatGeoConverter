using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataModel {

    public sealed class NGDecade : IEnumerable<NGIssue> {
        readonly List<NGIssue> _issues = new List<NGIssue>();
        readonly string _name;

        public string Name { get { return _name; } }

        public NGDecade( IEnumerable<NGIssue> issues, string name ) {
            _issues.AddRange( issues.OrderBy( _ => _.ReleaseDate ) );
            _name = name;
        }

        public static NGDecade Parse( string path ) {
            return new NGDecade( 
                    Directory.GetDirectories( path ).Select( NGIssue.Parse ), 
                    name: path.Substring( path.LastIndexOf( Path.DirectorySeparatorChar ) + 1 ) );
        }

        public IEnumerator<NGIssue> GetEnumerator() {
            return _issues.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
