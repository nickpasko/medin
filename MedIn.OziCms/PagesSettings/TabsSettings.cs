using System.Collections.Generic;
using MedIn.OziCms.PagesSettings.Forms;

namespace MedIn.OziCms.PagesSettings
{
	public class TabsSettings
	{
		public List<FieldSettings> Fields { get; set; }
		public string Name { get; set; }
		public int Sort { get; set; }
	}
}
