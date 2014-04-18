using System;

namespace DataModelLoader.NGModel.Extensions {
	public sealed class PageExceptions {
		public string basename { get; set; }

		public Correction[] corrections { get; set; }

		public PageRun[] page_runs { get; set; }

		public LargePage[] large_pages{ get; set; }
	}
}

