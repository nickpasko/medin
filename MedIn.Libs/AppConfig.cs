using System.Web.Mvc;
using MedIn.Libs.Services;

namespace MedIn.Libs
{
	public static class AppConfig
	{
		public static readonly string BasePathForImages = "BasePathForImages";
		public static readonly string NoImageRelativePath = "NoImageRelativePath";

		public static string GetValue(string name)
		{
			var settings = DependencyResolver.Current.GetService<ISettingsProvider>();
			return settings.GetValue(name);
		}
	}
}