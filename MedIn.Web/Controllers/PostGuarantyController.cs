using System.Web.Mvc;
using MedIn.OziCms.Services;
using MedIn.OziCms.ViewModels;
using MedIn.Web.Core;
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
                EmailService.SendMail(new SendMailViewModel
                {
                    From = SiteConfig.Get(SiteConfig.Keys.PostGuarantyFromEmail),
                    Bcc = SiteConfig.Get(SiteConfig.Keys.PostGuarantyBccEmail),
                    SenderName = SiteConfig.Get(SiteConfig.Keys.PostGuarantyName),
                    Subject = SiteConfig.Get(SiteConfig.Keys.PostGuarantyName),
                    To = Constants.CompanyEmail,
                    TemplateName = MVC.Mails.Views.PostGuarantyMail,
                    ViewModel = vm
                }, ControllerContext);
                
                return View(MVC.Mails.Views.SuccessSend);
            }
            return View(vm);
        }

        //public virtual ActionResult SuccessSend()
        //{
        //    return View();
        //}

    }
}
