using NUnit.Framework;
using System;
using Utilities;

namespace Tests {
	[TestFixture]
	public class UriPathTests {
		[Test]
		public void ShouldCombineUrlComponents() {
			Assert.That( UriPath.Combine( "html", "1880s", "index.html" ),
				Is.EqualTo( "html/1880s/index.html" ),
				"Did not combine paths." );
		}
	}
}

