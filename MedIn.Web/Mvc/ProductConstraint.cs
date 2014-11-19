using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MedIn.Domain.Entities;

namespace MedIn.Web.Mvc
{
    public class ProductConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            try
            {
                var db = DependencyResolver.Current.GetService<DataModelContext>();
                var categories = db.CreateObjectSet<Category>().ToList();
                var products = db.CreateObjectSet<Product>().ToList();
                var webContext = DependencyResolver.Current.GetService<IWebContext>();
                if (routeDirection == RouteDirection.IncomingRequest)
                {
                    var path = (string)values["categoryAndProduct"];
                    Product result = null;
                    if (!string.IsNullOrEmpty(path))
                    {
                        var parts = path.Split('/').Select(s => s.ToLower()).ToList();
                        var category = CheckIncomingRouteValues(parts.Take(parts.Count-1).ToList(), categories);
                        result = CheckProduct(parts, products, category);
                        webContext.ViewBag.CurrentCategory = category;
                        webContext.ViewBag.CurrentProduct = result;
                        if (category != null)
                        {
                            var location = db.Locations.FirstOrDefault(p => p.Alias == category.Alias);
                            if (location != null)
                                webContext.Location = location;
                        }
                    }
                    return result != null;
                }

                if (routeDirection == RouteDirection.UrlGeneration)
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;

        }

        private Product CheckProduct(IList<string> parts, IList<Product> Products, Category category)
        {
            if (category == null)
                return null;
            var alias = parts.LastOrDefault();
            if (string.IsNullOrEmpty(alias))
                return null;
            return Products.FirstOrDefault(Product => Product.Visibility && Product.CategoryId == category.Id && Product.Alias == alias);
        }

        private Category CheckIncomingRouteValues(IList<string> parts, IList<Category> categories)
        {
            return CheckParts(parts, categories);
        }

        private Category CheckParts(IList<string> parts, IList<Category> categories)
        {
            if (parts.Count == 0) return null;
            var page = categories.FirstOrDefault(l => l.Visibility && l.ParentId == null && l.Alias == parts[0]);
            return page != null ? CheckPartsRecursively(page, parts, 1, categories) : null;
        }

        private Category CheckPartsRecursively(Category current, IList<string> parts, int index, IList<Category> categories)
        {
            if (index >= parts.Count)
                return current;
            var page = categories.FirstOrDefault(l => l.Visibility && l.ParentId == current.Id && l.Alias == parts[index]);
            return page != null ? CheckPartsRecursively(page, parts, index + 1, categories) : null;
        }
    }
}