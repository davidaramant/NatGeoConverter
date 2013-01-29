using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Html {
    public sealed class HtmlWriter : ITagWriter {
        readonly StringWriter _writer;

        public HtmlWriter( StringWriter writer ) {
            _writer = writer;

            _writer.WriteLine( "<html>" );
        }

        public void Dispose() {
            _writer.WriteLine( "</html>" );
        }

        public void Tag( string tag, string content ) {
            _writer.WriteLine( String.Format( "<{0}>{1}</{0}>", tag, content ) );
        }

        public void WriteLine( string line ) {
            _writer.WriteLine( line );
        }

        public ITagWriter SubTag( string tag ) {
            return new TagWriter( parent: this, name: tag );
        }
    }

    public sealed class TagWriter : ITagWriter {
        private readonly ITagWriter _parent;
        private readonly string _name;

        public TagWriter( ITagWriter parent, string name ) {
            _parent = parent;
            _name = name;

            _parent.WriteLine( "<" + name + ">" );
        }

        public void Tag( string tag, string content ) {
            _parent.WriteLine( String.Format( "<{0}>{1}</{0}>", tag, content ) );
        }

        public void WriteLine( string line ) {
            _parent.WriteLine( line );
        }

        public ITagWriter SubTag( string tag ) {
            return new TagWriter( parent: this, name: tag );
        }

        public void Dispose() {
            _parent.WriteLine( "</" + _name + ">" );
        }
    }

    public interface ITagWriter : IDisposable {
        ITagWriter SubTag( string tag );
        void Tag( string tag, string content );
        void WriteLine( string line );
    }

}
