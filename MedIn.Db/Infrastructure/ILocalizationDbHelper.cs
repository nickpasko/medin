using System;

namespace MedIn.Db.Infrastructure
{
	public interface ILocalizationDbHelper
	{
		string Get(string lang, Guid? key);
		void Set(string lang, Guid key, string value);
	}
}
