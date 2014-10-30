using System;

namespace MedIn.Libs.Services
{
	public interface ILogger
	{
		void LogException(Exception exception, string message = null);
	}
}