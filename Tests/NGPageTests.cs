﻿using DataModel;
using NUnit.Framework;

namespace Tests {
    [TestFixture]
    public class NGPageTests {
        [TestCase( "NGM_1888_10_001_4.jpg", 1888, 10, "", 1, false )]
        [TestCase( "1930_01_Florida.jpg", 1930, 1, "", 0, true )]
        [TestCase( "NGM_1893_04A_001_4.jpg", 1893, 4, "A", 1, false )]
        [TestCase( "NGM_SW_1993_11_001_4.jpg", 1993, 11, "", 1, false )]
        [TestCase( "NGM_SE_1981_02_001_4.jpg", 1981, 02, "", 1, false )]
        [TestCase( "NGM_1897_07_08_001_4.jpg", 1897, 07, "", 1, false )]
        public void ShouldParseOutPageNumber( string path, int year, int month, string qualifier, int pageNumber, bool special ) {
            var page = new NGPage( path, "." );
            Assert.That( page.IsSpecial, Is.EqualTo( special ), "Did not determine if it was special." );
            Assert.That( page.Year, Is.EqualTo( year ), "Did not parse year." );
            Assert.That( page.Month, Is.EqualTo( month ), "Did not parse month." );
            Assert.That( page.IssueQualifier, Is.EqualTo( qualifier ), "Did not parse qualifier." );
            Assert.That( page.PageNumber, Is.EqualTo( pageNumber ), "Did not parse number." );
        }

        [TestCase( "NGM_1888_10_063_801_4.jpg", "063 801" )]
        [TestCase( "1899_10_NORTH CAROLINA TENNESSEE.jpg", "NORTH CAROLINA TENNESSEE" )]
        [TestCase( "NGM_SM_1893_04_4.jpg", "4" )]
        public void ShouldFigureOutDisplayNameForSpecialPage( string fileName, string displayName ) {
            var page = new NGPage( fileName, "." );
            Assert.That( page.DisplayName, Is.EqualTo( displayName ) );
        }

        [Test]
        public void ShouldFigureOutRelativePath() {
            var page = new NGPage(
                path: @"G:\National Geographic\JPG\188x\18881001\NGM_1888_10_001_4.jpg",
                basePath: @"G:\National Geographic" );

            Assert.That( page.RelativePath, Is.EqualTo( @"JPG\188x\18881001\NGM_1888_10_001_4.jpg" ),
                "Did not parse out relative path." );
        }
    }
}
