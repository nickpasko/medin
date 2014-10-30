using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using MedIn.Db.Entities;
using MedIn.Libs;
using MedIn.OziCms.ViewModels;

namespace MedIn.OziCms.Helpers
{
	public static class MvcExtensions
	{
		public static IHtmlString Submit(this HtmlHelper helper, object value = null, string viewname = null)
		{
			object model;
			if (value == null)
			{
				model = new SubmitViewModel();
			}
			else
			{
				var s = value as string;
				model = s != null ? new SubmitViewModel { Value = s } : value;
			}
			string result;
			if (string.IsNullOrWhiteSpace(viewname))
			{
				var m = (SubmitViewModel) model;
				Debug.Assert(m != null);
				var tag = new TagBuilder("input");
				tag.Attributes.Add("type", "submit");
				tag.AddCssClass("button");
				if (!string.IsNullOrWhiteSpace(m.Value)) tag.Attributes.Add("value", m.Value);
				if (!string.IsNullOrWhiteSpace(m.Class)) tag.AddCssClass(m.Class);
				if (!string.IsNullOrWhiteSpace(m.Name)) tag.Attributes.Add("name", m.Name);
				if (!string.IsNullOrWhiteSpace(m.Id)) tag.Attributes.Add("id", m.Id);
				result = tag.ToString(TagRenderMode.SelfClosing);
			}
			else
			{
				result = ViewHelpers.RenderPartialViewToString(viewname, new ViewDataDictionary(model), helper.ViewContext.Controller.ControllerContext);
			}
			return helper.Raw(result);
		}


		public static string Image(this UrlHelper helper, IFileEntity file, params string[] parameters)
		{
			var relativePath = file == null ? null : file.Name;
			return helper.Image(relativePath, parameters);
		}

		public static IHtmlString Image(this HtmlHelper helper, IFileEntity file, params string[] parameters)
		{
			var relativePath = file == null ? null : file.Name;
			return helper.Image(relativePath, parameters);
		}

		public static IHtmlString Image(this HtmlHelper helper, IFileEntity file, object attrs, params string[] parameters)
		{
			var relativePath = file == null ? null : file.Name;
			return helper.Image(relativePath, attrs, parameters);
		}

		//public static string Image(this UrlHelper helper, IFileEntity file, ProcessingStrategy strategy = null)
		//{
		//	if (file == null)
		//		return null;
		//	return helper.Image(file.Name, strategy);
		//}

		//public static string Image(this UrlHelper helper, IFileEntity file, int width, int height, ProcessingStrategy strategy = null)
		//{
		//	return helper.Image((file == null) ? null : file.Name, width, height, strategy);
		//}

		//public static IHtmlString Image(this HtmlHelper helper, IFileEntity file, ProcessingStrategy strategy = null, object attrs = null)
		//{
		//	return helper.Image((file == null) ? null : file.Name, strategy, attrs);
		//}

		//public static IHtmlString Image(this HtmlHelper helper, IFileEntity file, int width, int height, ProcessingStrategy strategy = null, object attrs = null)
		//{
		//	return helper.Image((file == null) ? null : file.Name, width, height, strategy, attrs);
		//}

		//public static string ImageFor(this UrlHelper helper, IFileEntity image, int width, int height, bool proportional)
		//{
		//	return ImageFor(helper.RequestContext.HttpContext, image, width, height, proportional);
		//}

		//private static string ImageFor(HttpContextBase context, IFileEntity image, int width, int height, bool proportional)
		//{
		//	var model = DefaultFileService.GetImageUrl(image.Name, width, height, proportional);
		//	var src = UrlHelper.GenerateContentUrl(model, context);
		//	return src;
		//}

		//public static MvcHtmlString ImageFor(this HtmlHelper helper, IFileEntity image, int width, int height, bool proportional, object htmlAttributes = null)
		//{
		//	var src = ImageFor(helper.ViewContext.HttpContext, image, width, height, proportional);
		//	var img = new TagBuilder("img");
		//	img.Attributes.Add("src", src);
		//	if (width != 0 && height != 0)
		//	{
		//		if (width > height)
		//		{
		//			SafeAddAttr(img, new KeyValuePair<string, string>("width", width.ToString(CultureInfo.InvariantCulture)));
		//		}
		//		else
		//		{
		//			SafeAddAttr(img, new KeyValuePair<string, string>("height", height.ToString(CultureInfo.InvariantCulture)));
		//		}
		//	}
		//	else
		//	{
		//		if (width != 0)
		//		{
		//			SafeAddAttr(img, new KeyValuePair<string, string>("width", width.ToString(CultureInfo.InvariantCulture)));
		//		}

		//		if (height != 0)
		//		{
		//			SafeAddAttr(img, new KeyValuePair<string, string>("height", height.ToString(CultureInfo.InvariantCulture)));
		//		}
		//	}
		//	if (htmlAttributes != null)
		//	{
		//		var attrs = new RouteValueDictionary(htmlAttributes);
		//		img.MergeAttributes(attrs, true);
		//	}
		//	return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));
		//}

		private static void SafeAddAttr(TagBuilder tag, KeyValuePair<string, string> attr, bool replace = true)
		{
			if (attr.Key == null || attr.Value == null)
				return;
			if (tag.Attributes.ContainsKey(attr.Key) && replace)
				tag.Attributes[attr.Key] = attr.Value;
			if (!tag.Attributes.ContainsKey(attr.Key))
			{
				tag.Attributes.Add(attr);
			}
		}
	}
}
