using System.Linq;
using System.Web.Mvc;
using MedIn.Domain.Entities;
using MedIn.OziCms.ViewModels;
using MedIn.Web.Core;
using MedIn.Web.Models;
using MedIn.Web.Mvc;

namespace MedIn.Web.Controllers
{
    public partial class CatalogsController : BaseController
    {
        public virtual ActionResult Index(int? page)
        {
            if (page == 1)
                return RedirectToActionPermanent(MVC.Catalogs.Index());
            var model = new ListItemsViewModel<Catalog>
            {
                Items = GetSortedVisible<Catalog>(page ?? 1, Constants.CatalogsPageSize),
                Pager = new PagerViewModel
                {
                    Action = p => MVC.Catalogs.Index(p),
                    ItemsCount = GetSortedVisible<Catalog>().Count(),
                    Page = page ?? 1,
                    PageSize = Constants.CatalogsPageSize
                }
            };
            if (page.HasValue && page.Value > 1 && !model.Items.Any())
                return HttpNotFound();
            return View(model);
        }
    }
}
