using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace MedIn.Web.Mvc
{
	public static class RouteExtensions
	{
		public static Route GetRouteByAlias(this RouteCollection table, string alias)
		{
			var loweredAlias = alias.ToLower();
			var element = table.Select(r => new
				{
					Route = (Route)r,
					Alias = (string)(((Route)r).Defaults != null ? ((Route)r).Defaults["location"] : null)
				}).Where(item => item.Alias != null).FirstOrDefault(item => item.Alias == loweredAlias);
			if (element != null)
			{
				return element.Route;
				//var route = element.Route;
				//var result = new Route(route.Url, route.Defaults, route.Constraints, route.DataTokens, route.RouteHandler);
				//if (route.Defaults.ContainsKey("alias"))
				//{
				//	result.Defaults["alias"] = alias;
				//}
				//else
				//{
				//	result.Defaults.Add("alias", alias);
				//}
				//return result;
			}
			return null;
		}
	}
}