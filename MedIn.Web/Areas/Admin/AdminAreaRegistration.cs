using System.Web.Mvc;

namespace MedIn.Web.Areas.Admin
{
	public class AdminAreaRegistration : AreaRegistration
	{
		public readonly string Namespace = "MedIn.Web.Areas.Admin.Controllers";

		public override string AreaName
		{
			get
			{
				return "Admin";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
            context.MapRoute(
                "AdminDefaultStartPage",
                "admin",
                new { controller = MVC.Admin.Locations.Name, action = MVC.Admin.Locations.ActionNames.Index },
                new[] { Namespace }
            );
            context.MapRoute(
                "AdminDefaultAction",
                "admin/{controller}",
                new { action = MVC.Admin.Locations.ActionNames.Index },
                new[] { Namespace }
            );
            context.MapRoute(
                "AdminDefault",
                "admin/{controller}/{action}/{id}",
                new { controller = MVC.Admin.Locations.Name, action = MVC.Admin.Locations.ActionNames.Index, id = UrlParameter.Optional },
                new[] { Namespace }
            );
		}
	}
}
