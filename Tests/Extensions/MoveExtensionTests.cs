using NUnit.Framework;
using System;
using System.Collections.Generic;
using Utilities.EnumerableExtensions;

namespace Tests {
	[TestFixture]
	public class MoveExtensionTests {
		[Test]
		public void ShouldHandleNegativeAdjustment() {
			var list = new List<int>{ 1, 2, 3, 4, 5 };

			list.Move( 2, -1 );

			Assert.That( list, Is.EqualTo( new[]{ 1, 3, 2, 4, 5 } ) );
		}

		[Test]
		public void ShouldHandlePositiveAdjustment() {
			var list = new List<int>{ 1, 2, 3, 4, 5 };

			list.Move( 2, 1 );

			Assert.That( list, Is.EqualTo( new[]{ 1, 2, 4, 3, 5 } ) );
		}
	}
}

