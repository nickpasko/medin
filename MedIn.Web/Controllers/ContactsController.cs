using System.Web.Mvc;
using MedIn.OziCms.Mvc;
using MedIn.OziCms.Services;
using MedIn.OziCms.ViewModels;
using MedIn.Web.Mvc;
using MedIn.Libs;
using MedIn.Web.Models;

namespace MedIn.Web.Controllers
{
	public partial class ContactsController : BaseController
    {
		public virtual ActionResult Index()
        {
	        var model = new FeedbackViewModel();
            return View(model);
        }

		[AjaxPost]
		[ValidateAntiForgeryToken]
		public virtual ActionResult WriteUs(FeedbackViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					EmailService.SendMail(new SendMailViewModel
					{
						From = AppConfig.GetValue("ContactsFromEmail"),
						SenderName = AppConfig.GetValue("ContactsFromName"),
						Subject = AppConfig.GetValue("ContactsSubject"),
						To = AppConfig.GetValue("ContactsToEmail"),
						ViewModel = model,
						TemplateName = MVC.Mails.Views.Contacts
					}, ControllerContext);
					ModelState.Clear();
					return PartialView(new FeedbackViewModel { ToastrMessage = L("MessageSent"), Success = true });
				}
				catch
				{
					return PartialView(new FeedbackViewModel { ToastrMessage = L("UnknownError"), Success = false });
				}
			}
			return PartialView(model);
		}
    }
}
