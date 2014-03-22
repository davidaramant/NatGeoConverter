using System.Collections.Generic;
using Utilities;

namespace DataModel {
	public interface IDecade : IEnumerable<IIssue> {
		string DisplayName { get; }
		string IndexFileName { get; }
		string RelativeIndexPath { get; }
		IPage PreviewPage { get; }
	}
}