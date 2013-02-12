using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Html {
    public sealed class HtmlWriter : ITagWriter {
        readonly StringWriter _writer;

        public HtmlWriter( StringWriter writer, string title, string pathModifier = "" ) {
            _writer = writer;
            _writer.WriteLine( @"<!DOCTYPE html>" );
            _writer.WriteLine( @"<html>" );
            using( var head = new TagWriter( parent: this, name: "head" ) ) {
                head.WriteLine( @"<meta charset=""utf-8""/>" );
                head.Tag( "title", title );
                head.WriteLine( 
                    String.Format( @"<link rel=""stylesheet"" type=""text/css"" href=""{0}"" />", 
                    Path.Combine( pathModifier, "css", "natgeo.css" ) ) );
            }
            _writer.WriteLine( "<body>" );
        }

        public void Dispose() {
            _writer.WriteLine( "</body" );
            _writer.WriteLine( "</html>" );
        }

        public void WriteLine( string line ) {
            _writer.WriteLine( line );
        }
    }

    public sealed class TagWriter : ITagWriter {
        private readonly ITagWriter _parent;
        private readonly string _name;

        public TagWriter( ITagWriter parent, string name, string property = "" ) {
            _parent = parent;
            _name = name;

            if( !String.IsNullOrWhiteSpace( property ) ) {
                property = " " + property;
            }

            _parent.WriteLine( String.Format( "<{0}{1}>", name, property ) );
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
        void WriteLine( string line );
    }

}
