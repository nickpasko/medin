using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using MedIn.Web.App_Start;

namespace MedIn.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            RegisterUsers();
        }

        public void RegisterUsers()
        {
            //if (Membership.GetUser("admin") == null)
            //{
            //    Membership.CreateUser("admin", "qweqwe");
            //}
            //if (Membership.GetUser("admin") == null)
            //{
            //    Membership.CreateUser("admin", "qweqwe");
            //}
            //if (!Roles.RoleExists("admin"))
            //{
            //    Roles.CreateRole("admin");
            //}
            //if (!Roles.IsUserInRole("admin", "admin"))
            //{
            //    Roles.AddUserToRole("admin", "admin");
            //}

            var admin = Membership.GetUser("admin");
            if (admin != null) admin.UnlockUser();
#if DEBUG
            var p = admin.ResetPassword();
            admin.ChangePassword(p, "lofiquv^");
#endif
        }
    }
}