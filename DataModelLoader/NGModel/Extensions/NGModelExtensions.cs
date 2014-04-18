using System;
using System.Text.RegularExpressions;
using Utilities;

namespace DataModelLoader.NGModel.Extensions {
	public static class NGModelExtensions {
		private static string ConvertToJson( string ngFormat ) {
			if( ngFormat.StartsWith( ";" ) ) {
				ngFormat = ngFormat.Substring( 1, ngFormat.Length - 1 );
			}

			ngFormat = Regex.Replace( ngFormat, @"#:(\w+)=>?", @"""$1"": " );

			return String.Format( "{{{0}}}", ngFormat.Replace( '@', ',' ).Replace( ';', ',' ) );
		}

		public static PageExceptions GetPageExceptions( this issues issue ) {
			return JsonDeserializer.Deserialise<PageExceptions>( ConvertToJson( issue.page_exceptions ) );
		}
	}
}

