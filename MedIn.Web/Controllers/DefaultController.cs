using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MedIn.Domain.Entities;
using MedIn.Web.Mvc;
using MedIn.Web.Models;

namespace MedIn.Web.Controllers
{
	public partial class DefaultController : BaseController
	{
		public virtual ActionResult Index()
		{
			return View();
		}

		public virtual ActionResult Page()
		{
			return View();
		}

		private Gallery GetGalleryById(int id)
		{
			var db = DependencyResolver.Current.GetService<DataModelContext>();
			var result = db.Galleries.FirstOrDefault(g => g.Id == id);
			return result;
		}

	    public virtual ActionResult MainSlider()
	    {
	        return PartialView();
	    }

		[ChildActionOnly]
		public virtual ActionResult Gallery(int id)
		{
			var model = GetGalleryById(id);
			return PartialView(model);
		}

		[ChildActionOnly]
		public virtual ActionResult GalleryFiles(int id, int? count = null)
		{
			var gallery = GetGalleryById(id);
			IEnumerable<File> model = gallery.Files.OrderBy(file => file.Sort);
			if (count.HasValue)
			{
				model = model.Take(count.Value);
			}
			return PartialView(model);
		}

		[ChildActionOnly]
		public virtual ActionResult TopMenu()
		{
			var model = GetSortedVisible<Location>().Where(location => location.ParentId == null && location.ShowInMenu);
			return PartialView(model.ToArray());
		}

		[ChildActionOnly]
		public virtual ActionResult LeftMenu()
		{
			var model = WebContext.Location.Root.Children.Where(location => location.Visibility && location.ShowInMenu).OrderBy(location => location.Sort);
            return PartialView(model.ToArray());
		}
		[ChildActionOnly]
		public virtual ActionResult NewsBlock()
		{
			var news = GetVisible<News>().OrderByDescending(n => n.PublishDate).Take(3);
			return PartialView(news);
		}

       [ChildActionOnly]
		public virtual ActionResult ChildrenBlock(string alias, string imgUrl)
		{
			var page = GetByAlias<Location>(alias);

			if (page == null)
				return new EmptyResult();
			var model = new ChildrenViewModel
				{
					Location = page,
					ImgUrl = imgUrl
				};
			return PartialView(model);
		}

	}
}
