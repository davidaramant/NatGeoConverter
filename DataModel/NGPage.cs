using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DataModel {
    [DebuggerDisplay( "{ToString()}" )]
    public sealed class NGPage {
        private string _path;

        public bool IsSpecial {
            get;
            private set;
        }

        public int Number {
            get;
            private set;
        }

        public NGPage( string path ) {
            _path = path;

            var fileName = Path.GetFileName( path );


        }

        public override string ToString() {
            return _path;
        }
    }
}
