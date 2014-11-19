using System.Linq;
using System.Web.Mvc;
using MedIn.Domain.Entities;
using MedIn.Web.Core;
using MedIn.Web.Models;
using MedIn.Web.Mvc;

namespace MedIn.Web.Controllers
{
    public partial class ProductsController : BaseController
    {
        private readonly ITabService _tabService;

        public ProductsController(ITabService tabService)
        {
            _tabService = tabService;
        }

        public virtual ActionResult Categories(string categories)
        {
            Category curCategoryEntity = WebContext.ViewBag.CurrentCategory;
            WebContext.ViewBag.Categories = GetSortedVisible<Category>().Where(x => x.Parent == null).ToList();
            var vm = new CategoriesViewModels
            {
                Products = curCategoryEntity.With(x => x.AllProducts)
            };
            return View(vm);
        }

        public virtual ActionResult Details(string categoryAndProduct)
        {
            WebContext.ViewBag.Categories = GetSortedVisible<Category>().Where(x => x.Parent == null).ToList();

            var vm = new ProductViewModel
            {
                Categories = GetSortedVisible<Category>().Where(x => x.Parent == null).ToList(),
                Tabs = _tabService.GetProductTabs(WebContext.ViewBag.CurrentProduct, ControllerContext)
            };
            return View(vm);
        }
    }
}
