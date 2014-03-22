using System;
using System.Collections.Generic;
using Utilities;

namespace DataModel {
	public interface IIssue : IEnumerable<IPage>  {
		DateTime ReleaseDate { get; }
		IPage CoverPage { get; }
		string ShortDisplayName { get; }
		string LongDisplayName { get; }
		string IndexFileName { get; }
		string IndexPath { get; }
		string UrlRelativeToParent { get; }
	}
}

