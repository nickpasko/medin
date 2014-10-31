using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using MedIn.Domain.Entities;

namespace MedIn.Web.Mvc
{
    public static class UrlExtensions
    {
        public static string ForPage(this UrlHelper helper, Location location)
        {
            if (location == null)
                return null;
            RouteTable.Routes.GetRouteByAlias(location.Alias);
            {
                var url = new StringBuilder();
                var item = location;
                do
                {
                    url.Insert(0, string.Format("/{0}", item.Alias));
                    item = item.Parent;
                } while (item != null);
                return url.ToString();
            }
        }

        public static string ForPage(this UrlHelper helper, string alias)
        {
            var locations = DependencyResolver.Current.GetService<DataModelContext>().Locations;
            var location = locations.FirstOrDefault(l => l.Alias == alias);
            return helper.ForPage(location);
        }
    }
}