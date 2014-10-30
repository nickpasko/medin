using System.Collections.Generic;
using System.Linq;
using MedIn.OziCms.PagesSettings.Forms;

namespace MedIn.OziCms.PagesSettings
{
	public class FormSettings
	{
		public List<TabsSettings> Tabs { get; set; }
		public bool? Localizable { get; set; }

		public List<FieldSettings> Fields
		{
			get { return Tabs.OrderBy(t => t.Sort).SelectMany(t => t.Fields).ToList(); }
		}
	}
}
