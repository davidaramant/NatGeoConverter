using System;

namespace DataModel {
	public interface IIssue  {
		int Id { get; }
		DateTime ReleaseDate { get; }
		int DecadeId { get; }
		IDecade Decade { get; }
		string ShortDisplayName { get; }
		string LongDisplayName { get; }
		string IndexFileName { get; }
		string DirectoryName { get; }

		int CoverPageId { get; }
		IPage CoverPage { get; }
	}
}