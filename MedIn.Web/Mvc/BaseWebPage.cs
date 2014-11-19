using System;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace MedIn.Web.Mvc
{
    public abstract class BaseWebPage<TModel> : WebViewPage<TModel>
    {
        protected IWebContext WebContext { get { return DependencyResolver.Current.GetService<IWebContext>(); } }

        public IHtmlString ByCondition(bool condition1, string trueValue1, bool condition2 = false, string falseValue2 = null)
        {
            if (condition1)
                return MvcHtmlString.Create(trueValue1);
            if (condition2)
                return MvcHtmlString.Create(falseValue2);
            return MvcHtmlString.Create(string.Empty);
        }

        public IHtmlString GetLstCategoryClass(bool mainCondition, IHtmlString activeTrail, IHtmlString firstLast)
        {
            if (!string.IsNullOrEmpty(activeTrail.ToString()))
                activeTrail = MvcHtmlString.Create(string.Format(" {0}", activeTrail));
            if (!string.IsNullOrEmpty(firstLast.ToString()))
                firstLast = MvcHtmlString.Create(string.Format("{0} ", firstLast));
            if (mainCondition)
                return MvcHtmlString.Create("expanded" + activeTrail);
            return MvcHtmlString.Create(firstLast + "leaf" + activeTrail);
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