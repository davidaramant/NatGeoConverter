using System;
using System.Text.RegularExpressions;
using Utilities;
using DataModel;

namespace DataModelLoader.NGModel.Extensions {
	public static class NGModelExtensions {
		public static string ConvertToJson( string ngFormat ) {
			if( ngFormat.StartsWith( ";" ) ) {
				ngFormat = ngFormat.Substring( 1, ngFormat.Length - 1 );
			}

			ngFormat = Regex.Replace( ngFormat, @"#:(\w+)=>?", @"""$1"": " );

			return String.Format( "{{{0}}}", ngFormat.Replace( '@', ',' ).Replace( ';', ',' ) );
		}

		public static PageExceptions GetPageExceptions( this issues issue ) {
			return JsonUtility.Deserialise<PageExceptions>( ConvertToJson( issue.page_exceptions ) );
		}

		public static int GetSearchTime( this Issue issue )
		{
			return 
				issue.ReleaseDate.Year * 10000 +
				issue.ReleaseDate.Month * 100 +
				issue.ReleaseDate.Day;
		}

        public static int ToSearchTime( this DateTime dateTime ) {
            return 
                dateTime.Year * 10000 +
                dateTime.Month * 100 +
                dateTime.Day;
        }
	}
}

