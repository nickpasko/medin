using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MedIn.Web.Mvc
{
	public class DotOrgRoute : Route
	{
		public DotOrgRoute(string url, IRouteHandler routeHandler) : base(url, routeHandler)
		{
		}

		public DotOrgRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler) : base(url, defaults, routeHandler)
		{
		}

		public DotOrgRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler) : base(url, defaults, constraints, routeHandler)
		{
		}

		public DotOrgRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler) : base(url, defaults, constraints, dataTokens, routeHandler)
		{
		}

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {

			//if ("ru".Equals(values["lang"]))
			//{
			//	values.Remove("lang");
			//}
			var result = base.GetVirtualPath(requestContext, values);

			if (result != null && !string.IsNullOrEmpty(result.VirtualPath))
            {
                var parts = result.VirtualPath.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries);
                var path = parts[0];
                if (!path.EndsWith("/"))
                {
                    path += "/";
                }
                var query = string.Empty;
                if (parts.Length > 1)
                {
                    query = "?" + parts[1];
                }

                result.VirtualPath = path + query;

            }
            return result;
        }
	}
}