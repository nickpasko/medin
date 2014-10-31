using System;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace MedIn.Web.Mvc
{
    public abstract class BaseWebPage<TModel> : WebViewPage<TModel>
    {
        protected IWebContext WebContext { get { return DependencyResolver.Current.GetService<IWebContext>(); } }

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
    }

    public abstract class BaseWebPage : BaseWebPage<object>
    {
    }
}