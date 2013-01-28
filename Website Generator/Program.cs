using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DataModel;

namespace Website_Generator {
    class Program {
        readonly static string _basePath = Path.Combine( "G:", "National Geographic" );
        readonly static string _baseJpgPath = Path.Combine( _basePath, "JPG" );


        static void Main( string[] args ) {
            try {
                DoStuff();
            } catch( Exception e ) {
                WL( new String( '-', 79 ) );
                WL( e.ToString() );
            }

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

            foreach( var decade in decades ) {
                foreach( var issue in decade ) {
                    foreach( var page in issue ) {
                        if( page.Failed ) {
                            WL( page.ToString() );
                        }
                    }
                }
            }
        }
    }
}
