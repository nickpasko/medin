using System.Diagnostics;
using System.Web.Mvc;
using MedIn.OziCms.Memberships;
using IOFile = System.IO.File;


namespace MedIn.Web.Areas.Admin.Controllers
{
    [OziAuthorize]
    public partial class UtilsController : Controller
    {
        public virtual ActionResult ClearApplicationCache()
        {
            var server = System.Web.HttpContext.Current.Server;
            System.IO.StreamReader streamReader = System.IO.File.OpenText(server.MapPath("~/Web.config"));
            string configContent = streamReader.ReadToEnd();
            streamReader.Close();

            var streamWriter = new System.IO.StreamWriter(server.MapPath("~/Web.config"));
            streamWriter.Write(configContent);
            streamWriter.Close();

            var request = System.Web.HttpContext.Current.Request;
            Debug.Assert(request.UrlReferrer != null);
            return Redirect(request.UrlReferrer.ToString());
        }

        public virtual ActionResult NotRealizedYet()
        {
            return View();
        }
    }
}
     
