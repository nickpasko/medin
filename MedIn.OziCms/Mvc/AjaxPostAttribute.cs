using System.Web;
using System.Web.Mvc;

namespace MedIn.OziCms.Mvc
{
	public class AjaxPostAttribute : AjaxOnlyAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);
			if (filterContext.HttpContext.Request.RequestType != "POST")
			{
				throw new HttpException(403, "Forbidden");
			}
		}
	}
}