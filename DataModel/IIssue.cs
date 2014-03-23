using System;

namespace DataModel {
	public interface IIssue  {
		DateTime ReleaseDate { get; }
		int DecadeId { get; }
		string ShortDisplayName { get; }
		string LongDisplayName { get; }
		string IndexFileName { get; }
		string DirectoryName { get; }
	}
}