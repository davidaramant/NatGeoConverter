namespace DataModel {
	public interface IPage {
		int Id { get; }
		int Number { get; }
		string DisplayName { get; }
		string IndexName { get; }
		string FileName { get; }

		string DecadeDirName { get; }
		string IssueDirName { get; }

		int IssueId { get; }
		int DecadeId { get; }

		int FullImageWidth { get; }
		int FullImageHeight { get; }

		int ThumbnailImageWidth { get; }
		int ThumbnailImageHeight { get; }

		int ThumbnailImageDisplayWidth { get; }
		int ThumbnailImageDisplayHeight { get; }
	}
}