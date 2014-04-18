using System;

namespace DataModelLoader.NGModel.Extensions {
	public sealed class LargePage {
		public int offset { get; set; }

		public int page_count{ get; set; }

		public float ratio{ get; set; }

		public string filename{ get; set; }
	}
}

