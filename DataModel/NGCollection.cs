using System;
using SQLite;
using DataModel.Database;
using System.Collections.Generic;
using Utilities;
using System.IO;
using Utilities.EnumerableExtensions;

namespace DataModel {
	public class NGCollection {
		readonly IProjectConfig _config;

		public NGCollection( IProjectConfig config ) {
			_config = config;
		}

		private SQLiteConnection Open()
		{
			return new SQLiteConnection( _config.DatabasePath );
		}

		public IEnumerable<NGDecade> GetAllDecades() {
			var dbDecades = new List<Decade>();
			return null;
		}
	}
}