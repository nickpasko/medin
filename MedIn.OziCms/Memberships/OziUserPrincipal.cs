using System.Security.Principal;
using System.Web.Security;

namespace MedIn.OziCms.Memberships
{
	public class OziUserPrincipal : IPrincipal
	{
		public OziUserPrincipal(IIdentity identity)
		{
			Identity = identity;
		}

		public bool IsInRole(string role)
		{
			return Identity.IsAuthenticated && Roles.IsUserInRole(Identity.Name, role);
		}

		public IIdentity Identity { get; private set; }
	}
}