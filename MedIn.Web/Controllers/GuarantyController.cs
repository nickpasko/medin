using System.Web.Mvc;
using MedIn.Web.Models;
using MedIn.Web.Mvc;

namespace MedIn.Web.Controllers
{
    public partial class GuarantyController : BaseController
    {
        public virtual ActionResult Guaranty()
        {
            return View();
        }

        
    }
}
