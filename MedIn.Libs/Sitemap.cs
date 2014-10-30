using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace MedIn.Libs
{
	public class Sitemap
	{
		private const string NodeName = "siteMapNode";
		private const string TitleAttribute = "title";
		private const string ActionAttribute = "action";
		private const string ControllerAttribute = "controller";
		private const string DescriptionAttribute = "description";

		private XDocument XDoc { get; set; }
		public List<SitemapElement> SitemapList { get; set; }
		private string SitemapFilePath { get; set; }

		public Sitemap(string path)
		{
			SitemapFilePath = HttpContext.Current.Server.MapPath(path);
			XDoc = XDocument.Load(SitemapFilePath);
			var q = XDoc.Root;
			SitemapList = GetChilds(q);
		}

		private static List<SitemapElement> GetChilds(XContainer element)
		{
			var childs = (from e in element.Elements(NodeName)
										   select new SitemapElement
										   {
											   Title = e.GetString(TitleAttribute),
											   Action = e.GetString(ActionAttribute),
											   Controller = e.GetString(ControllerAttribute),
											   Description = e.GetString(DescriptionAttribute),
											   Childs = GetChilds(e)
										   }).ToList();

			return childs;
		}
	}
}