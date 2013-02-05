using DataModel;
using DataModel.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Website_Generator {
    class Program {
        readonly static string _basePath = Path.Combine( "G:", "National Geographic" );
        readonly static string _baseJpgPath = Path.Combine( _basePath, "JPG" );
        readonly static string _baseHtmlPath = Path.Combine( _basePath, "HTML" );

        static void Main( string[] args ) {
            try {
                DoStuff();
            } catch( Exception e ) {
                WL( new String( '-', 79 ) );
                WL( e.ToString() );
            }

            WL( "Done" );
            Console.ReadKey();
        }

        static void WL() {
            Console.Out.WriteLine();
        }

        static void WL( string format, params object[] args ) {
            Console.Out.WriteLine( format, args );
        }

        static void DoStuff() {
            var decades = Directory.GetDirectories( _baseJpgPath ).Select( NGDecade.Parse ).ToArray();

            WL( "{0} decades", decades.Length );

            GenerateMainIndex( decades );
        }

        static void GenerateMainIndex( IEnumerable<NGDecade> decades ) {
            var sw = new StringWriter();
            using( var index = new HtmlWriter( sw, "National Geographic" ) ) {
                using( var previews = index.Div( className: "previews" ) ) {
                    foreach( var decade in decades.OrderBy( _ => _.Name ) ) {
                        using( var decadePreview = previews.Div( "previewBox" ) ) {
                            decadePreview.WriteLine( String.Format( @"<img src=""{0}"" alt=""{1}""/>", decade.First().Cover.FullPath, decade.Name ) );
                            decadePreview.Tag( "h2", decade.Name );
                        }
                    }
                }
            }

            File.WriteAllText( Path.Combine( _basePath, "index.html" ), sw.ToString() );
        }


    }
}
