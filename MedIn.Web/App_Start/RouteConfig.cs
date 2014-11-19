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

            routes.MapRoute("homepage", "", MVC.Default.Index(), "default");
            routes.MapRoute("homepage2", "", MVC.Default.Index(), "");
            routes.MapRoute("about","",MVC.AboutCompany.Index(),"about");

            routes.MapRoute("faq", "faq", MVC.Faq.Index(), "faq");
            routes.MapRoute("contacts", "contacts", MVC.Contacts.Index(), "contacts");
            routes.MapRoute("news", "about/news", MVC.News.Index(), "news");
            routes.MapRoute("news-item", "about/news/{id}", MVC.News.Details(), "news");
            routes.MapRoute("articles", "articles", MVC.Articles.Index(), "articles");
            routes.MapRoute("article", "articles/{alias}", MVC.Articles.Details(), "articles");
            routes.MapRoute("products", "products", MVC.Products.Categories(), "products", new { category = new CategoriesConstraint() }, new[] { "MedIn.Web.Controllers" });
            routes.MapRoute("productsSelector", "products/{*categoryAndProduct}", MVC.Products.Details(), "productsSelector", new { product = new ProductConstraint() }, new[] { "MedIn.Web.Controllers" });
            routes.MapRoute("categorySelector", "products/{*categories}", MVC.Products.Categories(), "categorySelector", new { category = new CategoriesConstraint() }, new[] { "MedIn.Web.Controllers" });

            var route = routes.MapRoute("static-pages", "{*location}", MVC.Default.Page(), new { localizationRedirectRouteName = "homepage" }, new { location = new LocationConstraints() });
			route.DataTokens["RouteName"] = "static-pages";

			routes.MapRoute("default-t4", "{controller}/{action}", MVC.Default.Index());
			routes.MapRoute("default", "{controller}/{action}/{id}", MVC.Default.Index());
			routes.MapRoute("default-aliased", "{controller}/{action}/{alias}", MVC.Default.Index());
		}
	}
}