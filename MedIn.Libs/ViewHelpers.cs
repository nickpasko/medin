using System.IO;
using System.Web.Mvc;
using System.Web.UI;

namespace MedIn.Libs
{
	public class ViewHelpers
	{
		public static string RenderPartialViewToString(string viewName, ViewDataDictionary viewData, ControllerContext controllerContext)
		{
			var viewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
			return RenderPartialViewToString(viewData, controllerContext, viewResult.View);
		}

		private static string RenderPartialViewToString(ViewDataDictionary viewData, ControllerContext controllerContext, IView view)
		{
			var writer = new HtmlTextWriter(new StringWriter());
			view.Render(new ViewContext(controllerContext, view, viewData, new TempDataDictionary(), writer), writer);
			var value = writer.InnerWriter.ToString();
			return value;
		}

	}
}