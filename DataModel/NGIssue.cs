﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DataModel {
    [DebuggerDisplay( "{ToString()}" )]
    public sealed class NGIssue : IEnumerable<NGPage> {
        readonly List<NGPage> _pages = new List<NGPage>();
        readonly List<NGPage> _specialPages = new List<NGPage>();

        readonly DateTime _releaseDate;

        public DateTime ReleaseDate { get { return _releaseDate; } }

        public NGPage Cover {
            get { return _pages[0]; }
        }

        public string DisplayName {
            get { return _releaseDate.ToString( "yyyy - MM - dd" ); }
        }

        public string Name {
            get { return _releaseDate.ToString( "yyyy-MM-dd" ); }
        }

        bool HasSpecialPages {
            get { return _specialPages.Any(); }
        }

        public NGIssue( IEnumerable<NGPage> pages, DateTime releaseDate ) {
            _pages.AddRange( pages.Where( p => !p.IsSpecial ).OrderBy( _ => _.FullPath ) );
            _specialPages.AddRange( pages.Where( p => p.IsSpecial ).OrderBy( _ => _.FullPath ) );
            _releaseDate = releaseDate;
        }

        public static NGIssue Parse( string path, string basePath ) {
            var dateMatch = Regex.Match( path, @"(\d{4})(\d{2})(\d{2})", RegexOptions.Compiled );

            if( !dateMatch.Success ) {
                throw new Exception( "Unknown date format: " + path );
            }

            var releaseDate = new DateTime(
                year: Int32.Parse( dateMatch.Groups[1].Value ),
                month: Int32.Parse( dateMatch.Groups[2].Value ),
                day: Int32.Parse( dateMatch.Groups[3].Value ) );

            return new NGIssue(
                    Directory.GetFiles( path, searchPattern: "*.jpg" ).OrderBy( _ => _ ).Select( pagePath => NGPage.Parse( pagePath, basePath: basePath ) ),
                    releaseDate: releaseDate );
        }

        public IEnumerator<NGPage> GetEnumerator() {
            return _pages.Concat( _specialPages ).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public string Serialize() {
            return "issue;" + _releaseDate.ToString( "s" );
        }

        public override string ToString() {
            return String.Format( "{0} {1} pages", DisplayName, _pages.Count() );
        }
    }
}
