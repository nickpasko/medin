using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using MedIn.Db.Entities;
using MedIn.OziCms.Mvc;

namespace MedIn.OziCms.PagesSettings.Lists
{
	public class FilterSettings : ColSettings
	{
		public string FilterField
		{
			get;
			set;
		}

		public IEnumerable<IEntity> FilterValues { get; set; }

		public override string Control
		{
			get
			{
				return ControlsNames.String;
			}
			set
			{
				base.Control = value;
			}
		}

		public override string HeaderControl
		{
			get
			{
				return ControlsNames.Filter;
			}
			set
			{
				base.HeaderControl = value;
			}
		}

		public override string HeaderValue()
		{
			return FilterField;
		}

		public override void AddRouteParameter(RouteValueDictionary parameters)
		{
			var value = HttpContext.Current.Request[FilterField];
			if (!string.IsNullOrEmpty(value))
			{
				parameters.Add(FilterField, value);
			}
		}
	}
}
