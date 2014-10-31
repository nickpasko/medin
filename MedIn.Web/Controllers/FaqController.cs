using System;
using System.Linq;
using System.Web.Mvc;
using MedIn.Domain.Entities;
using MedIn.OziCms.Mvc;
using MedIn.OziCms.Services;
using MedIn.OziCms.ViewModels;
using MedIn.Web.Mvc;
using MedIn.Libs;
using MedIn.Web.Models;
using Constants = MedIn.Web.Core.Constants;

namespace MedIn.Web.Controllers
{
	public partial class FaqController : BaseController
    {
		public virtual ActionResult Index(int? page)
		{
			if (page == 1)
				return RedirectToActionPermanent(MVC.Faq.Index());
			var items = GetSortedVisible<Question, long>(q => -q.PublishDate.Ticks, page ?? 1, Constants.QuestionsPageSize);
			var count = GetVisible<Question>().Count();
			var model = new ListItemsViewModel<Question>
			{
				Items = items,
				Pager = new PagerViewModel
				{
					Action = p => MVC.Faq.Index(p),
					ItemsCount = count,
					Page = page ?? 1,
					PageSize = Constants.QuestionsPageSize
				}
			};
			if (page.HasValue && page.Value > 1 && !model.Items.Any())
				return HttpNotFound();
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
					using (var db = new DataModelContext())
					{
						var question = db.CreateObject<Question>();
						question.PublishDate = DateTime.Now;
						question.QuestionText = model.Message;
						question.Email = model.Email;
						question.UserName = model.Username;
						db.Questions.AddObject(question);
						db.SaveChanges();
						model.Id = question.Id;
					}
					EmailService.SendMail(new SendMailViewModel
					{
						From = AppConfig.GetValue("FaqFromEmail"),
						SenderName = AppConfig.GetValue("FaqFromName"),
						Subject = AppConfig.GetValue("FaqSubject"),
						To = AppConfig.GetValue("FaqToEmail"),
						ViewModel = model,
						TemplateName = MVC.Mails.Views.Question
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
