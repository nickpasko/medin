using System.Linq;
using System.Web.Mvc;
using MedIn.Domain.Entities;
using MedIn.OziCms.ViewModels;
using MedIn.Web.Mvc;
using MedIn.Web.Core;
using MedIn.Web.Models;

namespace MedIn.Web.Controllers
{
    public partial class NewsController : BaseController
    {
        public virtual ActionResult Index(int? page)
        {
            if (page == 1)
                return RedirectToActionPermanent(MVC.News.Index());
	        var p = page ?? 1;
            var model = new ListItemsViewModel<News>
                            {
                                Items = GetVisible<News>().OrderByDescending(n => n.PublishDate).Skip((p-1)*Constants.NewsPageSize).Take(Constants.NewsPageSize),
                                Pager = new PagerViewModel
                                            {
												Action = pageIndex => MVC.News.Index(pageIndex),
                                                ItemsCount = GetVisible<News>().Count(),
												Page = p,
                                                PageSize = Constants.NewsPageSize
                                            }
                            };
            if (page.HasValue && page.Value > 1 && !model.Items.Any())
                return HttpNotFound();
			return View(model);
        }

        public virtual ActionResult Details(int id)
        {
            var item = GetById<News>(id);
            if (item == null)
                return HttpNotFound();
            var model = new ItemViewModel<News>
            {
                Item = item,
                ItemPage = GetItemPage(Constants.NewsPageSize,item)
            };
	        WebContext.Metadata = model.Item;
			WebContext.PushBreadcrumb(model.Item.Name ?? model.Item.PublishDate.ToString("D"), model.Item.Id.ToString());
            return View(model);
        }

    }
}
