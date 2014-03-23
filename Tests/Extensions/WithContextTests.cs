using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using Utilities.EnumerableExtensions;

namespace Tests {
	[TestFixture]
	public class WithContextTests {
		[Test]
		public void ShouldGetCorrectNumberOfElements() {
			var sequence = new[]{ "1", "2", "3" };

			var contextSequence = sequence.WithContext();

			Assert.That( contextSequence.Count(), Is.EqualTo( 3 ), 
				"Did not get expect number of context elements." );
		}

		[Test]
		public void ShouldGetCorrectElements() {
			var sequence = new[]{ "1", "2", "3" };

			var contextSequence = sequence.WithContext();

			TestContext( contextSequence, 0, null, "1", "2" );
			TestContext( contextSequence, 1, "1", "2", "3" );
			TestContext( contextSequence, 2, "2", "3", null );
		}

		void TestContext( 
			IEnumerable<Utilities.EnumerableExtensions.Extensions.ElementContext<string>> contextSequence, 
				int index, 
				string previous, 
				string current, 
				string next )
		{
			var context = contextSequence.ElementAt( index );

			Assert.That( context.Previous, Is.EqualTo( previous ), 
				"Did not get correct previous element for index {0}.", index );
			Assert.That( context.Current, Is.EqualTo( current ), 
				"Did not get correct current element for index {0}.", index );
			Assert.That( context.Next, Is.EqualTo(  next ), 
				"Did not get correct next element for index {0}.", index );
		}


	}
}

