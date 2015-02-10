using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MedIn.Domain.Entities;
using MedIn.OziCms.ViewModels;
using MedIn.Web.Core;
using MedIn.Web.Models;
using MedIn.Web.Mvc;

namespace MedIn.Web.Controllers
{
    public partial class ProjectsController : BaseController
    {
        public virtual ActionResult Index(int? page)
        {
            if (page == 1)
                return RedirectToActionPermanent(MVC.News.Index());
            var p = page ?? 1;
            var model = new ListItemsViewModel<Project>
            {
                Items = GetVisible<Project>().OrderByDescending(n => n.Sort).Skip((p - 1) * Constants.ProjectsPageSize).Take(Constants.ProjectsPageSize),
                Pager = new PagerViewModel
                {
                    Action = pageIndex => MVC.News.Index(pageIndex),
                    ItemsCount = GetVisible<Project>().Count(),
                    Page = p,
                    PageSize = Constants.ProjectsPageSize
                }
            };
            if (page.HasValue && page.Value > 1 && !model.Items.Any())
                return HttpNotFound();
            return View(model);
        }

        public virtual ActionResult Details(string alias)
        {
            var item = GetByAlias<Project>(alias);
            if (item == null)
                return HttpNotFound();
            var model = new ItemViewModel<Project>
            {
                Item = item,
                ItemPage = GetItemPage(Constants.ProjectsPageSize, item)
            };
            WebContext.Metadata = model.Item;
            WebContext.PushBreadcrumb(model.Item.Name ?? model.Item.Alias, model.Item.Id.ToString());
            return View(model);
        }

    }
}
