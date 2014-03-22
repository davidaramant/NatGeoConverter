using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Utilities.PathExtensions;

namespace DataModel {
    [DebuggerDisplay("{ToString()}")]
    public sealed class NGDecade : IEnumerable<NGIssue> {
        readonly List<NGIssue> _issues = new List<NGIssue>();

		readonly string _fullPath;

		public string DisplayName { get { return _fullPath.GetLastDirectory().Replace( "x", "0s"); } }

		public string IndexFileName { get { return _fullPath.GetLastDirectory() + ".html"; } }

		public string RelativeIndexUrl {
			get {return Path.Combine( "html", IndexFileName ); }
		}

		public string DirectoryName {
			get { return _fullPath.GetLastDirectory(); }
		}

		public string PreviewImagePath 
		{ 
			get { return _issues.First().Cover.RelativeThumbnailUrl; }
		}

		public NGDecade( IEnumerable<NGIssue> issues, string fullPath ) {
            _issues.AddRange( issues.OrderBy( _ => _.ReleaseDate ) );
			_fullPath = fullPath;
        }
			   

        public IEnumerator<NGIssue> GetEnumerator() {
            return _issues.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public override string ToString() {
			return String.Format( "{0}: {1} issues", DisplayName, _issues.Count() );
        }			
    }
}
