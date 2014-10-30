using System.Diagnostics;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web.Configuration;
using System.Web.Mvc;
using MedIn.Domain.Entities;
using MedIn.OziCms.Controllers;
using MedIn.OziCms.Memberships;
using MedIn.Web;

namespace MedIn.Web.Areas.Admin.Controllers
{
    [OziAuthorize(Roles = "admin, superadmin", LoginUrl = "/admin/account/login")]
    public partial class SiteSettingsController : OziController
    {
        const string PathTemplate = "~/App_Data/{0}.xml";

        public virtual ActionResult Emails()
        {
            var config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
            var settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
            Debug.Assert(settings != null);
            return View(settings.Smtp);
        }

        [HttpPost]
        public virtual ActionResult Emails(SmtpSection model)
        {
            var config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
            var settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
            if (settings != null)
            {
                if (model.DeliveryMethod == SmtpDeliveryMethod.Network)
                {
                    settings.Smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    settings.Smtp.Network.ClientDomain = model.Network.ClientDomain;
                    settings.Smtp.Network.DefaultCredentials = model.Network.DefaultCredentials;
                    settings.Smtp.Network.EnableSsl = model.Network.EnableSsl;
                    settings.Smtp.Network.Host = model.Network.Host;
                    settings.Smtp.Network.Password = model.Network.Password;
                    settings.Smtp.Network.Port = model.Network.Port;
                    settings.Smtp.Network.UserName = model.Network.UserName;
                }
                else if (model.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
                {
                    settings.Smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    settings.Smtp.SpecifiedPickupDirectory.PickupDirectoryLocation = model.SpecifiedPickupDirectory.PickupDirectoryLocation;
                }
            }
            config.Save();
            return View(model);
        }

        public virtual ActionResult Logs()
        {
            using (var db = new DataModelContext())
            {
                var list = db.EmailLogs.OrderByDescending(log => log.Date).ToList();
                return View(list);
            }
        }

        public virtual ActionResult General()
        {
            using (var db = new DataModelContext())
            {
                var model = db.SiteSettings.ToList();
                return View(model);
            }
        }

        [HttpPost]
        public virtual ActionResult General(FormCollection values)
        {
            using (var db = new DataModelContext())
            {
                var settings = db.SiteSettings.ToList();
                foreach (string key in values.Keys)
                {
                    var value = values[key];
                    var s = settings.FirstOrDefault(setting => setting.Name == key);
                    if (s == null)
                        continue;
                    s.Value = value;
                }
                db.SaveChanges();
                return RedirectToAction(MVC.Admin.SiteSettings.General());
            }
        }
    }
}
