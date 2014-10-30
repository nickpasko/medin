using System;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using MedIn.Libs.Services;

namespace MedIn.OziCms.Mvc
{
	public abstract class DotOrgViewPage<TModel> : WebViewPage<TModel>
	{
		private ILocalizationProvider _lang;
		public ILocalizationProvider Lp
		{
			get
			{
				return _lang ?? (_lang = DependencyResolver.Current.GetService<ILocalizationProvider>());
			}
		}

		protected abstract IDotOrgWebContext DotOrgWebContext { get; }

		public IHtmlString ByCondition(bool condition, string value)
		{
			return MvcHtmlString.Create(condition ? value : string.Empty);
		}

		private static readonly object O = new object();
		public HelperResult RenderSection(string sectionName, Func<object, HelperResult> defaultContent)
		{
			return IsSectionDefined(sectionName) ? RenderSection(sectionName) : defaultContent(O);
		}

		public HelperResult RedefineSection(string sectionName)
		{
			return RedefineSection(sectionName, null);
		}

		public HelperResult RedefineSection(string sectionName, Func<object, HelperResult> defaultContent)
		{
			if (IsSectionDefined(sectionName))
			{
				DefineSection(sectionName, () => Write(RenderSection(sectionName)));
			}
			else if (defaultContent != null)
			{
				DefineSection(sectionName, () => Write(defaultContent(O)));
			}
			return new HelperResult(_ => { });
		}

		//public string MetaTitle
		//{
		//	get { return ViewBag.MetaTitle == null && string.IsNullOrEmpty(WebContext.Metadata.MetaTitle) ? Constants.DefaultMetaTitle : string.Format(Constants.DefaultMetaTitleTemplate, ViewBag.MetaTitle ?? WebContext.Metadata.MetaTitle); }
		//}

		//public string MetaKeywords
		//{
		//	get { return ViewBag.MetaKeywords ?? WebContext.Metadata.MetaKeywords; }
		//}

		//public string MetaDescription
		//{
		//	get { return ViewBag.MetaDescription ?? WebContext.Metadata.MetaDescription; }
		//}

		//public object SelectedClass(bool condition)
		//{
		//	if (condition)
		//		return new {@class = "selected"};
		//	return null;
		//}

		//protected void PageContent()
		//{
		//	Html.RenderAction(MVC.Default.PageContent());
		//}
	}

	public abstract class DotOrgViewPage : DotOrgViewPage<object>
	{
	}
}