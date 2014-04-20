using NUnit.Framework;
using System;
using Utilities;

namespace Tests {
	[TestFixture]
	public sealed class JsonDeserializerTests {
		sealed class Test {
			public string Name { get; set; }
		}

		[Test]
		public void ShouldDeserializeJson() {
			var json = @"{ ""Name"": ""Dog"" }";

			var deserialized = JsonUtility.Deserialise<Test>( json );

			Assert.That( deserialized, Is.Not.Null, "Did not return an object." );
			Assert.That( deserialized.Name, Is.EqualTo( "Dog" ), "Did not read object properties." );
		}
	}
}

