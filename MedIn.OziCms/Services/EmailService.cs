using System;
using System.Net.Mail;
using System.Web.Mvc;
using MedIn.Libs;
using MedIn.OziCms.ViewModels;

namespace MedIn.OziCms.Services
{
	public class EmailService
	{
		private static readonly object LockObject = new object();
		
		public static bool SendMail(SendMailViewModel model, ControllerContext controllerContext = null, IEmailLogService logger = null)
		{
			logger =  logger ?? DependencyResolver.Current.GetService<IEmailLogService>();
			try
			{
				var message = new MailMessage();

				var toes = model.To.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
				foreach (var to in toes)
				{
					message.To.Add(to);
				}
				message.From = model.FromAddress;
				message.BodyEncoding = System.Text.Encoding.UTF8;
				message.IsBodyHtml = true;
				if (!string.IsNullOrEmpty(model.Bcc))
				{
					toes = model.Bcc.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
					foreach (var to in toes)
					{
						message.Bcc.Add(to);
					}
				}
				message.Subject = model.Subject;

				if (controllerContext != null)
				{
					if (model.Files != null)
					{
						foreach (var file in model.Files)
						{
							var attachment = new Attachment(controllerContext.HttpContext.Server.MapPath(file.Name))
								{
									Name = file.SourceName
								};
							message.Attachments.Add(attachment);
						}
					}
					var viewData = new ViewDataDictionary(model.ViewModel);
					message.Body = ViewHelpers.RenderPartialViewToString(model.TemplateName, viewData, controllerContext);
				}
				else
				{
					message.Body = model.Body;
					if (model.Files != null)
					{
						foreach (var file in model.Files)
						{
							var attachment = new Attachment(file.Name) { Name = file.SourceName };
							message.Attachments.Add(attachment);
						}
					}
				}

				lock (LockObject)
				{
					var client = new SmtpClient();
					if (client.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
					{
						client.EnableSsl = false;
					}
					client.Send(message);
				}
				logger.Write(model.From, model.To, model.Subject, message.Body, null);
				return true;
			}
			catch (Exception exception)
			{
				logger.Write(model.From, model.To, model.Subject, model.TemplateName, exception);
				return false;
			}
		}
	}

	public interface IEmailLogService
	{
		void Write(string from, string to, string subject, string content, Exception exception);
	}
}
