using System;

namespace DataModelLoader.NGModel.Extensions {
	public sealed class Correction {
		public int search_time { get; set; }

		public string op { get; set; }

		public string filename { get; set; }

		public CorrectionOperation GetOperation() {
			switch( op ) {
				case "remove_image":
					return CorrectionOperation.RemoveImage;
				case "move_image":
					return CorrectionOperation.MoveImage;
				case "unnumbered_image":
					return CorrectionOperation.UnnumberedImage;
				case "not_large_page":
					return CorrectionOperation.NotLargePage;
				case "mark_large_page":
					return CorrectionOperation.MarkLargePage;
				case "move_map":
					return CorrectionOperation.MoveMap;
				case "add_new_image_before":
					return CorrectionOperation.AddNewImageBefore;
				case "insert_image_from_other_issue":
					return CorrectionOperation.InsertImageFromOtherIssue;
				case "remove_map":
					return CorrectionOperation.RemoveMap;
				case "delete_missing_numbered_pages":
					return CorrectionOperation.DeleteMissingNumberedPages;
				default:
					throw new InvalidOperationException( "Unknown operation." );
			}
		}
	}

	public enum CorrectionOperation {
		/// <summary>
		/// Useless.  Used to mark new images found in the app bundle.
		/// </summary>
		AddNewImageBefore,

		/// <summary>
		/// ?
		/// </summary>
		DeleteMissingNumberedPages,

		/// <summary>
		/// Do manually?
		/// </summary>
		InsertImageFromOtherIssue,

		/// <summary>
		/// Useless.  Marks a large page for their special snowflake app.
		/// </summary>
		MarkLargePage,

		/// <summary>
		/// Rearranges page order.
		/// </summary>
		MoveImage,

		/// <summary>
		/// Useless.  Moves the map to where it was in the issue.
		/// </summary>
		MoveMap,

		/// <summary>
		/// Useless.  The CNG app works on aspect ratio or something dumb.
		/// </summary>
		NotLargePage,

		/// <summary>
		/// Do this manually?
		/// </summary>
		RemoveImage,

		/// <summary>
		/// Do this manually?
		/// </summary>
		RemoveMap,

		/// <summary>
		/// Do "page numbers" actually matter? YES, unfortunately
		/// </summary>
		UnnumberedImage,
	}
}
