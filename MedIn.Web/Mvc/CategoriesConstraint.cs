using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MedIn.Domain.Entities;
using MedIn.Web.Models;

namespace MedIn.Web.Mvc
{
    public class CategoriesConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            try
            {
                var db = DependencyResolver.Current.GetService<DataModelContext>();
                var categories = db.Categories.ToList();
                var webContext = DependencyResolver.Current.GetService<IWebContext>();
                var bread = new List<BreadcrumbsViewModel>();
                if (routeDirection == RouteDirection.IncomingRequest)
                {
                    var path = (string)values["categories"];
                    var result = CheckIncomingRouteValues(path, categories, bread);
                    webContext.ViewBag.CurrentCategory = result;
                    var resultFlag = string.IsNullOrEmpty(path) || result != null;
                    if (result != null)
                    {
                        var location = db.Locations.FirstOrDefault(p => p.Alias == result.Alias);
                        if (location != null)
                        {
                            webContext.Location = location;
                        }
                    }
                    if (resultFlag)
                    {
                        //bread.ForEach(b => webContext.PushBreadcrumb(b.Text, b.Alias));
                    }
                    return resultFlag;
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

        private Category CheckIncomingRouteValues(string path, List<Category> categories, List<BreadcrumbsViewModel> crumbs)
        {
            if (string.IsNullOrEmpty(path))
                return null;
            var parts = path.Split('/').Select(s => s.ToLower()).ToList();
            return CheckParts(parts, categories, crumbs);
        }

        private Category CheckParts(IList<string> parts, IList<Category> categories, List<BreadcrumbsViewModel> crumbs)
        {
            var item = categories.FirstOrDefault(l => l.Visibility && l.ParentId == null && l.Alias == parts[0]);
            if (item != null)
            {
                crumbs.Add(new BreadcrumbsViewModel { Alias = item.Alias, Text = item.Name });
                return CheckPartsRecursively(item, parts, 1, categories, crumbs);
            }
            return null;
        }

        private Category CheckPartsRecursively(Category current, IList<string> parts, int index, IList<Category> categories, List<BreadcrumbsViewModel> crumbs)
        {
            if (index >= parts.Count)
                return current;
            var item = categories.FirstOrDefault(l => l.Visibility && l.ParentId == current.Id && l.Alias == parts[index]);
            if (item != null)
            {
                crumbs.Add(new BreadcrumbsViewModel { Alias = item.Alias, Text = item.Name });
                return CheckPartsRecursively(item, parts, index + 1, categories, crumbs);
            }
            return null;
        }
    }
}