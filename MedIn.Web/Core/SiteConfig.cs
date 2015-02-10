using System.Web.Mvc;
using MedIn.Libs.Services;

namespace MedIn.Web.Core
{
	public static class SiteConfig
	{
		public static string Get(string name)
		{
			var set = DependencyResolver.Current.GetService<ISettingsProvider>();
			return set.GetValue(name);
		}

		public static void Set(string name, string value)
		{
			var set = DependencyResolver.Current.GetService<ISettingsProvider>();
			set.SetValue(name, value);
		}

		public static class Keys
		{
            //public const string RegisterFromEmail = "RegisterFromEmail";
            //public const string RegisterBccEmail = "RegisterBccEmail";
            //public const string RegisterSenderName = "RegisterSenderName";
            //public const string RegisterSubject = "RegisterSubject";
            //public const string AvatarRelativePath = "AvatarRelativePath";

            //public const string SubscriptionFromEmail = "SubscriptionFromEmail";
            //public const string SubscriptionSenderName = "SubscriptionSenderName";
            public const string PostGuarantyFromEmail = "PostGuarantyFromEmail";
            public const string PostGuarantyBccEmail = "PostGuarantyBccEmail";
            public const string PostGuarantyName = "PostGuarantyName";
		}
	}
}