using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MedIn.OziCms.Memberships
{
    public class OziAuthorizeAttribute : AuthorizeAttribute
    {
		public string LoginUrl { get; set; }

		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			var authCookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
			if (authCookie == null)
			{
				httpContext.User = new OziUserPrincipal(new OziIdentity());
				return false;
			}
			var formsAuth = DependencyResolver.Current.GetService<IFormsAuthenticationService>();
			var principal = formsAuth.GetPrincipal(authCookie.Value);
			httpContext.User = principal;
			if (!string.IsNullOrEmpty(Roles))
			{
				var roles = Roles.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
				return roles.Any(principal.IsInRole);
			}
			return true;
		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			if (!string.IsNullOrEmpty(LoginUrl))
			{
				var returnUrl = string.Empty;
				if (filterContext.RequestContext.HttpContext.Request.Url != null)
				{
					returnUrl = "?returnUrl=" + filterContext.RequestContext.HttpContext.Request.Url.PathAndQuery;
				}
				filterContext.HttpContext.Response.Redirect(LoginUrl + returnUrl, true);
			}
			base.HandleUnauthorizedRequest(filterContext);
		}

    }
}
