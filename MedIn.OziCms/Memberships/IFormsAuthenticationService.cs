using System.Security.Principal;

namespace MedIn.OziCms.Memberships
{
	public interface IFormsAuthenticationService
	{
		void SignIn(string username, bool createPersistentCookie);
		void SignOut();
		IPrincipal GetPrincipal(string value);
	}
}