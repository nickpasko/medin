using System.Collections.Generic;

namespace MedIn.Libs
{
	public class SitemapElement
	{
		public List<SitemapElement> Childs { get; set; }
		public int Level { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }

		public string Controller { get; set; }
		public string Action { get; set; }
	}
}