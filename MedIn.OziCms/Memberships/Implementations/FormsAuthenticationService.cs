using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace MedIn.OziCms.Memberships.Implementations
{
	public class FormsAuthenticationService : IFormsAuthenticationService
	{
		public void SignIn(string username, bool createPersistentCookie)
		{
			var userData = new OziIdentity(username);
			var authTicket = new FormsAuthenticationTicket(1, username, DateTime.Now, CalculateCookieExpirationDate(), createPersistentCookie, userData.ToString());
			var ticket = FormsAuthentication.Encrypt(authTicket);
			var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticket);
			HttpContext.Current.Response.Cookies.Add(cookie);
		}

		public void SignOut()
		{
			FormsAuthentication.SignOut();
		}

		public IPrincipal GetPrincipal(string encryptedTicket)
		{
			return new OziUserPrincipal(GetIdentity(encryptedTicket));
		}

		private IIdentity GetIdentity(string encryptedTicket)
		{
			return new OziIdentity(Decrypt(encryptedTicket));
		}

		private static DateTime CalculateCookieExpirationDate()
		{
			return DateTime.Now.Add(FormsAuthentication.Timeout);
		}

		private static FormsAuthenticationTicket Decrypt(string encryptedTicket)
		{
			return FormsAuthentication.Decrypt(encryptedTicket);
		}
	}
}