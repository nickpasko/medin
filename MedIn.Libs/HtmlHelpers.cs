using System.Web;
using System.Web.Mvc;

namespace MedIn.Libs
{
    public static class HtmlExtensions
    {
		public const string NoImage = "~/styles/i/no-image.jpg";

		public static string PluralizeWord(this HtmlHelper htmlHelper, int count, string single, string several, string many)
		{
			var dec = (count % 100) / 10; // кол-во десятков
			var low = count % 10; // кол-во единиц
			if (low == 1 && dec != 1)
			{
				return single;
			}
			if (low > 1 && low < 5 && dec != 1)
			{
				return several;
			}
			return many;
		}

	    public static IHtmlString ByCondition(this HtmlHelper helper, bool condition, string value)
	    {
		    return helper.Raw(condition ? value : string.Empty);
	    }


		public static string Image(this UrlHelper helper, string relativePath, params string[] parameters)
		{
			relativePath = relativePath ?? NoImage;
			return string.Format("{0}?{1}", helper.Content(relativePath), string.Join("&", parameters));
		}

		public static IHtmlString Image(this HtmlHelper helper, string relativePath, params string[] parameters)
		{
			return helper.Image(relativePath, null, parameters);
		}

		public static IHtmlString Image(this HtmlHelper helper, string relativePath, object attrs = null, params string[] parameters)
		{
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
			var url = urlHelper.Image(relativePath, parameters);
			return GenerateImageElement(urlHelper, url, attrs, 0, 0);
		}


		//public static string Image(this UrlHelper helper, string relativePath, ProcessingStrategy strategy = null)
		//{
		//	if (string.IsNullOrEmpty(relativePath)) return null;
		//	var descriptor = new ImageDescriptor { SourceRelativeName = relativePath };
		//	GetImgUrl(strategy ?? ProcessingStrategy.DontProcess, descriptor);
		//	return helper.Content(descriptor.DestinationRelativeName);
		//}

		//public static string Image(this UrlHelper helper, string relativePath, int width, int height, ProcessingStrategy strategy = null)
		//{
		//	if (string.IsNullOrEmpty(relativePath)) return null;
		//	var descriptor = new ImageDescriptor { SourceRelativeName = relativePath };
		//	descriptor.Parameters[ImageDescriptor.ParametersNames.TargetWidth] = width;
		//	descriptor.Parameters[ImageDescriptor.ParametersNames.TargetHeight] = height;
		//	GetImgUrl(strategy ?? ProcessingStrategy.Resize, descriptor);
		//	return helper.Content(descriptor.DestinationRelativeName);
		//}

		//public static IHtmlString Image(this HtmlHelper helper, string relativePath, ProcessingStrategy strategy = null, object attrs = null)
		//{
		//	var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
		//	var url = urlHelper.Image(relativePath, strategy);
		//	return GenerateImageElement(urlHelper, url, attrs, 0, 0);
		//}

		//public static IHtmlString Image(this HtmlHelper helper, string relativePath, int width, int height, ProcessingStrategy strategy = null, object attrs = null)
		//{
		//	var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
		//	var url = urlHelper.Image(relativePath, width, height, strategy);
		//	return GenerateImageElement(urlHelper, url, attrs, width, height);
		//}


		private static IHtmlString GenerateImageElement(UrlHelper urlHelper, string imgUrl, object attrs, int w, int h)
		{
			var img = new TagBuilder("img");
			if (attrs != null)
			{
				img.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(attrs));
			}
			if (string.IsNullOrEmpty(imgUrl))
			{
				var url = urlHelper.Content("~/Images/main-pic-example.jpg");
				img.MergeAttribute("src", url);
				if (w != 0)
					img.MergeAttribute("width", w.ToString());
				if (h != 0)
					img.MergeAttribute("height", h.ToString());
			}
			else
			{
				img.MergeAttribute("src", imgUrl);
			}
			return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));
		}

		//private static void GetImgUrl(ProcessingStrategy strategy, ImageDescriptor descriptor)
		//{
		//	ImageHandler.Handle(descriptor, strategy.Handlers);
		//}
    }
}
