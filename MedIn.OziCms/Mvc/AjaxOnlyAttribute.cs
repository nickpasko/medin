using System.Web;
using System.Web.Mvc;

namespace MedIn.OziCms.Mvc
{
	public class AjaxOnlyAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (!filterContext.HttpContext.Request.IsAjaxRequest())
				throw new HttpException(403, "Forbidden");
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{

		}
	}
}
