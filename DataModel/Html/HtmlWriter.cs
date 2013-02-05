using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Html {
    public sealed class HtmlWriter : ITagWriter {
        readonly StringWriter _writer;

        public HtmlWriter( StringWriter writer, string title ) {
            _writer = writer;
            _writer.WriteLine( @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.1//EN"" ""http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"">" );
            _writer.WriteLine( @"<html xmlns=""http://www.w3.org/1999/xhtml"">" );
            using( var head = SubTag( "head" ) ) {
                head.WriteLine( @"<meta content=""text/html; charset=ISO-8859-1"" http-equiv=""content-type""/>" );
                head.Tag( "title", title );
                head.WriteLine( @"<link rel=""stylesheet"" type=""text/css"" href=""css/natgeo.css"" />" );
            }
            _writer.WriteLine( "<body>" );
        }

        public void Dispose() {
            _writer.WriteLine( "</body" );
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

        public DivWriter Div( string className ) {
            return new DivWriter( parent: this, className: className );
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

    public sealed class DivWriter : ITagWriter {
        private readonly ITagWriter _parent;

        public DivWriter( ITagWriter parent, string className ) {
            _parent = parent;
            _parent.WriteLine( String.Format( @"<div class=""{0}"">", className ) );
        }

        public ITagWriter SubTag( string tag ) {
            return new TagWriter( parent: this, name: tag );
        }

        public DivWriter Div( string className ) {
            return new DivWriter( parent: this, className: className );
        }

        public void Tag( string tag, string content ) {
            _parent.WriteLine( String.Format( "<{0}>{1}</{0}>", tag, content ) );
        }

        public void WriteLine( string line ) {
            _parent.WriteLine( line );
        }

        public void Dispose() {
            _parent.WriteLine( "</div>" );
        }
    }

    public interface ITagWriter : IDisposable {
        ITagWriter SubTag( string tag );
        void Tag( string tag, string content );
        void WriteLine( string line );
    }

}
