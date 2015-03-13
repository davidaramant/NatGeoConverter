using System;
using SQLite;
using System.Collections.Generic;
using Utilities;
using System.IO;
using Utilities.EnumerableExtensions;
using System.Linq;

namespace DataModel {
	public class NGCollection {
		readonly IProjectConfig _config;

		public NGCollection( IProjectConfig config ) {
			_config = config;
		}

		private SQLiteConnection Open() {
			return new SQLiteConnection( _config.DatabasePath );
		}

		public IEnumerable<Decade> GetAllDecades( bool hydrate = true ) {
			var decades = new List<Decade>();
			using( var db = Open() ) {
				decades.AddRange( db.Table<Decade>().OrderBy( d => d.DirectoryName ) );
				if( hydrate ) {
					foreach( var decade in decades ) {
						var previewPage = db.Query<Page>( "select * from Page where Id = ?", decade.PreviewPageId ).First();
						previewPage.Issue = db.Query<Issue>( "select * from Issue where Id = ?", previewPage.IssueId ).First();
						decade.PreviewPage = previewPage;
					}
				}
			}

			return decades;
		}

		public IEnumerable<Issue> GetAllIssuesInDecade( Decade decade ) {
			var issues = new List<Issue>();
			using( var db = Open() ) {
				issues.AddRange( db.Table<Issue>().Where( i => i.DecadeId == decade.Id ).OrderBy( i => i.ReleaseDate ) );
				foreach( var issue in issues ) {
					issue.Decade = db.Query<Decade>( "select * from Decade where Id = ?", issue.DecadeId ).First();
					issue.CoverPage = db.Query<Page>( "select * from Page where Id = ?", issue.CoverPageId ).First();
				}
			}
			return issues;
		}

        public IEnumerable<Issue> GetAllIssues( bool hydrateCoverPage = true ) {
			var issues = new List<Issue>();
			using( var db = Open() ) {
				issues.AddRange( db.Table<Issue>().OrderBy( i => i.ReleaseDate ) );
				foreach( var issue in issues ) {
					issue.Decade = db.Query<Decade>( "select * from Decade where Id = ?", issue.DecadeId ).First();
					if( hydrateCoverPage ) {
						issue.CoverPage = db.Query<Page>( "select * from Page where Id = ?", issue.CoverPageId ).First();
					}
				}
			}
			return issues;
		}

        public IEnumerable<Article> GetAllArticlesInIssue( Issue issue ) {
            var articles = new List<Article>();
            using( var db = Open() ) {
                articles.AddRange( db.Table<Article>().Where( a => a.IssueId == issue.Id ) );
                foreach( var article in articles.Where( a => a.SpecifiedPage ) ) {
                    var page = db.Query<Page>( "select * from Page where Id = ?", article.PageId ).FirstOrDefault();
                    if( page == null ) {
                        throw new ArgumentException( "Couldn't find page: " + article.PageId );
                    }
                    article.Page = page;
                }
                return articles.OrderBy( a => a.Page == null ? 0 : a.Page.Order ).ToList();
            }
        }

		public IEnumerable<Page> GetAllPagesInIssue( Issue issue ) {
			var pages = new List<Page>();
			using( var db = Open() ) {
				pages.AddRange( db.Table<Page>().Where( p => p.IssueId == issue.Id ).OrderBy( p => p.Order ) );
				foreach( var page in pages ) {
					page.Issue = issue;
				}
			}
			return pages;
		}
	}
}