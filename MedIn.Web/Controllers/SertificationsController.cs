using System.Linq;
using System.Web.Mvc;
using MedIn.Domain.Entities;
using MedIn.Web.Mvc;

namespace MedIn.Web.Controllers
{
    public partial class SertificationsController : BaseController
    {
        public virtual ActionResult Index()
        {
            return View(GetSortedVisible<Sertificate>().ToList());
        }
    }
}
