using System.Net.Mail;
using MedIn.Db.Entities;

namespace MedIn.OziCms.ViewModels
{
	public class SendMailViewModel
	{
		public string To { get; set; }
		public string Bcc { get; set; }
		public string From { get; set; }
		public string SenderName { get; set; }
		public string Subject { get; set; }

		public MailAddress FromAddress
		{
			get { return new MailAddress(From, SenderName); }
		}

		public dynamic ViewModel { get; set; }
		public string TemplateName { get; set; }
		public string Body { get; set; }

		public IFileEntity[] Files { get; set; }
	}
}
