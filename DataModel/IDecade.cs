namespace DataModel {
	public interface IDecade {
		int Id { get; }
		string DirectoryName { get; }
		string DisplayName { get; }
		string IndexFileName { get; }

		IPage PreviewPage { get; }
	}
}