using System;
using System.Text.RegularExpressions;

namespace DataModelLoader.NGModel.Extensions {
	public static class NGModelExtensions {
		public static string ToJson( this issues issue ) {
			var ngFormat = issue.page_exceptions;

			if( ngFormat.StartsWith( ";" ) ) {
				ngFormat = ngFormat.Substring( 1, ngFormat.Length - 1 );
			}

			ngFormat = Regex.Replace( ngFormat, @"#:(\w+)=>?", @"""$1"": " );

			return String.Format( "{{{0}}}", ngFormat.Replace( '@', ',' ).Replace( ';', ',' ) );
		}
	}
}

