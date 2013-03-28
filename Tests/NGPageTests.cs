using DataModel;
using NUnit.Framework;

namespace Tests {
    [TestFixture]
    public class NGPageTests {
        [TestCase( "NGM_1888_10_001_4.jpg", 1, "1", false )]
        [TestCase( "1930_01_Florida.jpg", null, "Florida", true )]
        [TestCase( "NGM_1893_04A_001_4.jpg", 1, "1", false )]
        [TestCase( "NGM_SW_1993_11_001_4.jpg", 1, "1", false )]
        [TestCase( "NGM_SE_1981_02_001_4.jpg", 1, "1", false )]
        [TestCase( "NGM_1897_07_08_001_4.jpg", 1, "1", false )]
        [TestCase( "NGM_1888_10_063_801_4.jpg", 63, "63", false )]
        [TestCase( "1899_10_NORTH CAROLINA TENNESSEE.jpg", null, "NORTH CAROLINA TENNESSEE", true )]
        [TestCase( "NGM_SM_1893_04_4.jpg", 4, "4", false )]
        public void ShouldParseOutPageNumber( string path, int? pageNumber, string displayName, bool special ) {
            var page = NGPage.Parse( path, "." );
            Assert.That( page.PageNumber, Is.EqualTo( pageNumber ), "Did not figure out page number" );
            Assert.That( page.DisplayName, Is.EqualTo( displayName ), "Did figure out display name." );
            Assert.That( page.IsSpecial, Is.EqualTo( special ), "Did not determine if it was special." );
        }
        [Test]
        public void ShouldFigureOutRelativePath() {
            var page = NGPage.Parse(
                fullPath: @"G:\National Geographic\JPG\188x\18881001\NGM_1888_10_001_4.jpg",
                basePath: @"G:\National Geographic" );

            Assert.That( page.RelativePath, Is.EqualTo( @"JPG\188x\18881001\NGM_1888_10_001_4.jpg" ),
                "Did not parse out relative path." );
        }
    }
}
