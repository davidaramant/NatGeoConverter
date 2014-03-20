using System;
using System.IO;
using NUnit.Framework;

using Utilities.Extensions;

namespace Tests.Extensions {
	[TestFixture]
	public class PathExtensionsTests {
		static string GetRootPath() {
			if( Environment.OSVersion.Platform == PlatformID.Win32NT ) {
				return "G:";
			}
			return "/";
		}


		[Test]
		public void ShouldFigureOutRelativePath() {
			var relativePath = Path.Combine( "JPG", "188x", "18881001", "NGM_1888_10_001_4.jpg" );
		    var basePath = Path.Combine( GetRootPath(), "National Geographic" );
			var fullPath = Path.Combine( basePath, relativePath );

			var actualRelativePath = fullPath.GetPathRelativeTo( basePath );

			Assert.That( actualRelativePath, Is.EqualTo( relativePath ),
			            "Did not parse out relative path." );
		}

		[TestCase("/some/path/with/stuff", "stuff")]
		[TestCase( "lonePath", "lonePath" )]
		[TestCase( "pathWith/slashOnEnd/", "slashOnEnd" )]
		public void ShouldGetLastDirectoryName( string input, string expectedOutput )
		{
			Assert.That( input.GetLastDirectory(), Is.EqualTo( expectedOutput ),
				"Did not get expected output." );
		}
	}
}

