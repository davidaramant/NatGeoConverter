using System;

namespace DataModelLoader.NGModel {
	public sealed class articles {
		public string display_name { get; set; }

		public int id { get; set; }

		public int issue_id{ get; set; }

		public int page_count{ get; set; }

		public int search_time{ get; set; }

		public int start_page_offset { get; set; }

		public string summary { get; set; }
	}
}

