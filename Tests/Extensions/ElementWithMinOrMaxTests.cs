using NUnit.Framework;
using System;
using Utilities.EnumerableExtensions;

namespace Tests {
	[TestFixture]
	public sealed class ElementWithMinOrMaxTests {
		sealed class T {
			public int Value;
			public string Name;
		}

		[Test]
		public void ShouldGetElementWithMaxValue() {
			var data = new [] {
				new T { Value = 1, Name = "One" },
				new T { Value = 3, Name = "Three" },
				new T { Value = 2, Name = "Two" },
			};

			var maxElement = data.ElementWithMax( t => t.Value );

			Assert.That( maxElement.Name, Is.EqualTo( "Three" ), "Did not get max element." );
		}

		[Test]
		public void ShouldGetElementWithMinValue() {
			var data = new [] {
				new T { Value = 1, Name = "One" },
				new T { Value = 3, Name = "Three" },
				new T { Value = 2, Name = "Two" },
			};

			var maxElement = data.ElementWithMin( t => t.Value );

			Assert.That( maxElement.Name, Is.EqualTo( "One" ), "Did not get min element." );
		}
	}
}

