using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using DataModel;

namespace Tests {
    [TestFixture]
    public class NGPageTests {
        [TestCase( "NGM_1888_10_001_4.jpg", 1888, 10, "", 1, false )]
        [TestCase( "1930_01_Florida.jpg", 1930, 1, "", 0, true )]
        [TestCase( "NGM_1893_04A_001_4.jpg", 1893, 4, "A", 1, false )]
        public void ShouldParseOutPageNumber( string path, int year, int month, string qualifier, int pageNumber, bool special ) {
            var page = new NGPage( path );
            Assert.That( page.IsSpecial, Is.EqualTo( special ), "Did not determine if it was special." );
            Assert.That( page.Year, Is.EqualTo( year ), "Did not parse year." );
            Assert.That( page.Month, Is.EqualTo( month ), "Did not parse month." );
            Assert.That( page.IssueQualifier, Is.EqualTo( qualifier ), "Did not parse qualifier." );
            Assert.That( page.PageNumber, Is.EqualTo( pageNumber ), "Did not parse number." );
        }
    }
}
