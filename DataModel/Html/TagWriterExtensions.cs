using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Html {
    public static class TagWriterExtensions {
        public static ITagWriter Div( this ITagWriter writer, string className ) {
            return new TagWriter( parent: writer, name: "div", property: String.Format( @"class=""{0}""", className ) );
        }

        public static ITagWriter Link( this ITagWriter writer, string link ) {
            return new TagWriter( parent: writer, name: "a", property: String.Format( @"href=""{0}""", link ) );
        }

        public static void Img( this ITagWriter writer, string link, string altText ) {
            writer.WriteLine( String.Format( @"<img src=""{0}"" alt=""{1}""/>", link, altText ) );
        }

        public static void H2( this ITagWriter writer, string text ) {
            writer.WriteLine( String.Format( "<h2>{0}</h2>", text ) );
        }

        public static void Tag( this ITagWriter writer, string tag, string content ) {
            writer.WriteLine( String.Format( "<{0}>{1}</{0}>", tag, content ) );
        }
    }
}
