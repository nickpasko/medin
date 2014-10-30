using System.Web.Mvc;
using MedIn.Libs.Services;

namespace MedIn.Libs
{
	public static class LocalizationHelpers
	{
		public static string Localize(this string key)
		{
			var lang = DependencyResolver.Current.GetService<ILocalizationProvider>();
			return lang.GetMessage(key);
		}
	}
}