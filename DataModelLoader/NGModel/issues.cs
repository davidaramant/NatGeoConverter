using System;

namespace DataModelLoader.NGModel {
	public class issues {
		public int id { get; set; }

		/// <summary>
		/// Appears to be utterly worthless.
		/// </summary>
		public string display_name { get; set; }

		/// <summary>
		/// yyyyMMdd
		/// </summary>
		public int search_time { get; set; }

		/// <summary>
		/// Should be the number of images in the issue.
		/// </summary>
		public int page_count { get; set; }

		/// <summary>
		/// X + 1 is the image that is the first numbered page.
		/// </summary>
		public int numbered_page_offset { get; set; }

		/// <summary>
		/// Seems to be what it claims, but how is this useful????
		/// </summary>
		public int numbered_page_count { get; set; }

		/// <summary>
		/// The displayed page number in the issue does not start at one; it seems to be a continous sequence per a year.
		/// </summary>
		public int numbered_page_start_value { get; set; }

		public string page_exceptions { get; set; }
	}
}

