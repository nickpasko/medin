using System.Collections.Generic;

namespace MedIn.OziCms.PagesSettings.Forms
{
	public class CollectionSettings : FieldSettings
	{
		public override string Control { get { return "ozi_Collection"; } }
		public string ItemTitle { get; set; }
		public bool Sortable { get; set; }
		public bool ShowDelete { get; set; }
		public bool ShowAdd { get; set; }
		public List<FieldSettings> Fields { get; set; }
	}
}
