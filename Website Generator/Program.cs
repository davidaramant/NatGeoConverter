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

            using( var index = new HtmlWriter( sw ) ) {
                using( var head = index.SubTag( "head" ) ) {
                    head.Tag( "title", "National Geographic" );
                }
                using( var body = index.SubTag( "body" ) ) {
                    body.Tag( "h1", "Hello" );
                    body.Tag( "p", "World" );
                }
            }

            File.WriteAllText( Path.Combine( _basePath, "index.html" ), sw.ToString() );
        }


    }
}
