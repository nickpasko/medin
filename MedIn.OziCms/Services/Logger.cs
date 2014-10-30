using System;
using System.Web.Mvc;
using MedIn.Libs.Services;

namespace MedIn.OziCms.Services
{
	public class Logger : ILogger
	{
		public Logger()
		{
		}

		private static ILogger _instance;

		public static ILogger Instance 
		{
			get { return _instance ?? (_instance = DependencyResolver.Current.GetService<ILogger>()); }
		}

		public void LogException(Exception exception, string message = null)
		{
			
		}
	}
}