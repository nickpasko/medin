using System.Web.Routing;

namespace MedIn.OziCms.PagesSettings.Lists
{
	public class ColSettings
	{
		public string Name { get; set; }
		public string Title { get; set; }
		public int Width { get; set; }
		public bool Levels { get; set; }
		public virtual string Control { get; set; }
		public virtual string HeaderControl { get; set; }
		public string RoleName { get; set; }
		public string Filter { get; set; }
		public string Order { get; set; }

		public bool Localizable { get; set; }

		public virtual string HeaderValue()
		{
			return Title;
		}

		public virtual void AddRouteParameter(RouteValueDictionary parameters)
		{
		}

		public string GetFilterField()
		{
			if (Filter == "off")
				return null;
			if (Filter == "on" || string.IsNullOrEmpty(Filter))
				return Name;
			return Filter;
		}

		public string GetOrderField()
		{
			if (Order == "off")
				return null;
			if (Order == "on" || string.IsNullOrEmpty(Order))
				return Name;
			return Order;
		}
	}
}
