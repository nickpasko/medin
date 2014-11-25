using System.Linq;
using System.Web.Mvc;
using MedIn.Domain.Entities;
using MedIn.OziCms.ViewModels;
using MedIn.Web.Mvc;
using MedIn.Web.Core;
using MedIn.Web.Models;

namespace MedIn.Web.Controllers
{
	public partial class ArticlesController : BaseController
	{
		public virtual ActionResult Index(int? page)
		{
			if (page == 1)
				return RedirectToActionPermanent(MVC.Articles.Index());
			var model = new ListItemsViewModel<Article>
							{
								Items = GetSortedVisible<Article>(page ?? 1, Constants.ArticlesPageSize),
								Pager = new PagerViewModel
											{
												Action = p => MVC.Articles.Index(p),
												ItemsCount = GetSortedVisible<Article>().Count(),
												Page = page ?? 1,
												PageSize = Constants.ArticlesPageSize
											}
							};
			if (page.HasValue && page.Value > 1 && !model.Items.Any())
				return HttpNotFound();
			return View(model);
		}

        //public virtual ActionResult Details(string alias)
        //{
        //    var item = GetByAlias<Article>(alias);
        //    if (item == null)
        //        return HttpNotFound();
        //    var model = new ItemViewModel<Article>()
        //                    {
        //                        Item = item,
        //                        ItemPage = GetItemPage(Constants.NewsPageSize, item)
        //                    };
        //    WebContext.Metadata = model.Item;
        //    WebContext.PushBreadcrumb(model);
        //    return View(model);
        //}
	}
}
