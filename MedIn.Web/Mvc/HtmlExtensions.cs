using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MedIn.Db.Entities;
using MedIn.Domain.Entities;

namespace MedIn.Web.Mvc
{
	public static class HtmlExtensions
	{
		public static IHtmlString PageLink(this HtmlHelper helper, Location location, string content = null, object attrs = null)
		{
			var result = new TagBuilder("a");
			result.MergeAttribute("href", new UrlHelper(helper.ViewContext.RequestContext).ForPage(location));
            result.InnerHtml = content ?? location.Name;
			if (attrs != null)
			{
				result.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(attrs));
			}
			return MvcHtmlString.Create(result.ToString(TagRenderMode.Normal));
		}

		public static IHtmlString PageLink(this HtmlHelper helper, string alias, string content = null, object attrs = null)
		{
			var locations = DependencyResolver.Current.GetService<DataModelContext>().Locations.ToList();
			var page = locations.FirstOrDefault(l => l.Visibility && l.Alias == alias);
			return helper.PageLink(page, content, attrs);
		}

		public static string UrlForFile(this UrlHelper helper, IFileEntity file, bool isPrivate)
		{
			if (file == null)
				return string.Empty;
			//var action = isPrivate ? MVC.Download.File(file.Id) : MVC.Download.Index(file.Id);
			//var url = helper.ActionAbsolute(action);
			//return url;
			return helper.Content(file.Name);
		}

		public static MvcHtmlString FileFor(this HtmlHelper helper, IFileEntity file, string defaultText, bool isPrivate = false)
		{
			if (file == null)
				return MvcHtmlString.Create(string.Empty);
			var a = new TagBuilder("a");

			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
			var url = urlHelper.UrlForFile(file, isPrivate);
			a.Attributes.Add("href", url);
			a.SetInnerText(file.SourceName ?? defaultText);
			var result = new StringBuilder();
			result.Append(a);
			result.Append(" (");
			if (file.Size.HasValue)
			{
				var span = new TagBuilder("span");
				span.SetInnerText(FileSizeString(file));
				//result.Append("&nbsp;(");
				result.Append(span);
				result.Append(",&nbsp;");
			}
			if (file.Date != DateTime.MinValue)
			{
				var span = new TagBuilder("span");
				span.SetInnerText(FileChange(file));
				//result.Append("&nbsp;(");
				result.Append(span);
				//result.Append(")");
			}
			result.Append(")");
			return MvcHtmlString.Create(result.ToString());
		}

		public static string FileSizeString(IFileEntity file)
		{
			Debug.Assert(file.Size != null);
			double dSize = file.Size.Value;
			var c = "Bytes";
			if (dSize > 1024 * 1024)
			{
				dSize = Math.Floor(10.0 * file.Size.Value / 1024.0 / 1024.0) / 10.0;
				c = "Mb";
			}
			else if (dSize > 1024)
			{
				dSize = Math.Floor(10.0 * file.Size.Value / 1024.0) / 10.0;
				c = "Kb";
			}
			return string.Format("{0}{1}", dSize, c);
		}


		public static string FileChange(IFileEntity file)
		{
			return string.Format("{0}", file.Date);
		}
	}
}