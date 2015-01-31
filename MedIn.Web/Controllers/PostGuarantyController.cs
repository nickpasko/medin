using System.Web.Mvc;
using MedIn.Web.Models;
using MedIn.Web.Mvc;

namespace MedIn.Web.Controllers
{
    public partial class PostGuarantyController : BaseController
    {
        [HttpGet]
        public virtual ActionResult PostGuaranty()
        {
            return View(new PostGuarantyViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult PostGuaranty(PostGuarantyViewModel vm)
        {
            if (ModelState.IsValid)
            {

            }
            return View(vm);
        }

    }
}
