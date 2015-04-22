using System.Web.Mvc;
using System.Web.Routing;
using MedIn.Web.Mvc;

namespace MedIn.Web.App_Start
{
	public static class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.Ignore("{resource}.axd/{*pathInfo}");
			routes.Ignore("favicon.png");
			routes.Ignore("favicon.ico");

            routes.MapRoute("homepage", "", MVC.Default.Index(), "default", new[] { "MedIn.Web.Controllers" });
            routes.MapRoute("homepage2", "", MVC.Default.Index(), "", new[] { "MedIn.Web.Controllers" });
            routes.MapRoute("about", "", MVC.AboutCompany.Index(), "about", new[] { "MedIn.Web.Controllers" });
            routes.MapRoute("service", "", MVC.Service.Index(), "service", new[] { "MedIn.Web.Controllers" });
            routes.MapRoute("guaranty", "service/guaranty", MVC.Guaranty.Guaranty(), "service", new[] { "MedIn.Web.Controllers" });
            routes.MapRoute("postguaranty", "service/postguaranty", MVC.PostGuaranty.PostGuaranty(), "postguaranty", new[] { "MedIn.Web.Controllers" });

            routes.MapRoute("faq", "faq", MVC.Faq.Index(), "faq", new[] { "MedIn.Web.Controllers" });
            routes.MapRoute("contacts", "contacts", MVC.Contacts.Index(), "contacts", new[] { "MedIn.Web.Controllers" });

            routes.MapRoute("projects", "projects", MVC.Projects.Index(), "projectList", new[] { "MedIn.Web.Controllers" });
            routes.MapRoute("project", "projects/{alias}", MVC.Projects.Details(), "projectDetails", new[] { "MedIn.Web.Controllers" });


            routes.MapRoute("news", "about/news", MVC.News.Index(), "news", new[] { "MedIn.Web.Controllers" });
            routes.MapRoute("news-item", "about/news/{id}", MVC.News.Details(), "news", new[] { "MedIn.Web.Controllers" });
            routes.MapRoute("articles", "articles", MVC.Articles.Index(), "articles", new[] { "MedIn.Web.Controllers" });
            routes.MapRoute("catalogs", "catalogs", MVC.Catalogs.Index(), "catalogs", new[] { "MedIn.Web.Controllers" });
            routes.MapRoute("sertifications", "about/sertifications", MVC.Sertifications.Index(), "sertifications", new[] { "MedIn.Web.Controllers" });
            routes.MapRoute("products", "products", MVC.Products.Categories(), "products", new { category = new CategoriesConstraint() }, new[] { "MedIn.Web.Controllers" });
            routes.MapRoute("productsSelector", "products/{*categoryAndProduct}", MVC.Products.Details(), "productsSelector", new { product = new ProductConstraint() }, new[] { "MedIn.Web.Controllers" });
            routes.MapRoute("categorySelector", "products/{*categories}", MVC.Products.Categories(), "categorySelector", new { category = new CategoriesConstraint() }, new[] { "MedIn.Web.Controllers" });

            var route = routes.MapRoute("static-pages", "{*location}", MVC.Default.Page(), new { localizationRedirectRouteName = "homepage" }, new { location = new LocationConstraints() });
			route.DataTokens["RouteName"] = "static-pages";

            routes.MapRoute("default-t4", "{controller}/{action}", MVC.Default.Index());
            routes.MapRoute("default", "{controller}/{action}/{id}", MVC.Default.Index(), new[] { "MedIn.Web.Controllers" });
            routes.MapRoute("default-aliased", "{controller}/{action}/{alias}", MVC.Default.Index());
		}
	}
}