using System;
using System.Linq;
using MedIn.Domain.Entities;
using MedIn.OziCms.Services;

namespace MedIn.Web.Core
{
	public class EmailLogsService : IEmailLogService
	{
		public void Write(string @from, string to, string subject, string content, Exception exception)
		{
			using (var db = new DataModelContext())
			{
				var bound = DateTime.Now.Subtract(TimeSpan.FromDays(7.0));
				var removeList = db.EmailLogs.Where(entry => entry.Date < bound).ToList();
				foreach (var entry in removeList)
				{
					db.EmailLogs.DeleteObject(entry);
				}

				var log = new EmailLog
					{
						Content = content,
						Date = DateTime.Now,
						From = @from,
						Status = exception == null,
						To = to,
						Subject = subject
					};
				if (exception != null)
				{
					log.Type = exception.GetType().Name;
					log.Message = exception.Message;
					log.StackTrace = exception.StackTrace;
					if (exception.InnerException != null)
					{
						log.Type += " / " + exception.InnerException.GetType().Name;
						log.Message += " / " + exception.InnerException.Message;
						log.StackTrace += Environment.NewLine + "==================" + exception.InnerException.StackTrace;
					}
				}
				db.EmailLogs.AddObject(log);
				db.SaveChanges();
			}
		}
	}
}
