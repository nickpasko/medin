using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedIn.Web.Mvc;

namespace MedIn.Web.Controllers
{
    public partial class ServiceController : BaseController
    {
        public virtual ActionResult Index()
        {
            return View();
        }

    }
}
