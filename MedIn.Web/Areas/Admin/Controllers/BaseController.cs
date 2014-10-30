using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedIn.Db.Entities;
using MedIn.Domain.Entities;
using MedIn.OziCms.Controllers;
using MedIn.OziCms.Memberships;

namespace MedIn.Web.Areas.Admin.Controllers
{
	[OziAuthorize(Roles = "admin", LoginUrl = "~/admin/account/login")]
    public class BaseController<T> : GenericController<T, DataModelContext> where T : class, IEntity, new()
    {
    }
}
