// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
#pragma warning disable 1591, 3008, 3009
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace MedIn.Web.Controllers
{
    public partial class DefaultController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public DefaultController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected DefaultController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Gallery()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Gallery);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult GalleryFiles()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GalleryFiles);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ChildrenBlock()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ChildrenBlock);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult SetLanguage()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SetLanguage);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public DefaultController Actions { get { return MVC.Default; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Default";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Default";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string Page = "Page";
            public readonly string MainSlider = "MainSlider";
            public readonly string Gallery = "Gallery";
            public readonly string GalleryFiles = "GalleryFiles";
            public readonly string TopMenu = "TopMenu";
            public readonly string LeftMenu = "LeftMenu";
            public readonly string NewsBlock = "NewsBlock";
            public readonly string ChildrenBlock = "ChildrenBlock";
            public readonly string SetLanguage = "SetLanguage";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string Page = "Page";
            public const string MainSlider = "MainSlider";
            public const string Gallery = "Gallery";
            public const string GalleryFiles = "GalleryFiles";
            public const string TopMenu = "TopMenu";
            public const string LeftMenu = "LeftMenu";
            public const string NewsBlock = "NewsBlock";
            public const string ChildrenBlock = "ChildrenBlock";
            public const string SetLanguage = "SetLanguage";
        }


        static readonly ActionParamsClass_Gallery s_params_Gallery = new ActionParamsClass_Gallery();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Gallery GalleryParams { get { return s_params_Gallery; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Gallery
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_GalleryFiles s_params_GalleryFiles = new ActionParamsClass_GalleryFiles();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GalleryFiles GalleryFilesParams { get { return s_params_GalleryFiles; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GalleryFiles
        {
            public readonly string id = "id";
            public readonly string count = "count";
        }
        static readonly ActionParamsClass_ChildrenBlock s_params_ChildrenBlock = new ActionParamsClass_ChildrenBlock();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ChildrenBlock ChildrenBlockParams { get { return s_params_ChildrenBlock; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ChildrenBlock
        {
            public readonly string alias = "alias";
            public readonly string imgUrl = "imgUrl";
        }
        static readonly ActionParamsClass_SetLanguage s_params_SetLanguage = new ActionParamsClass_SetLanguage();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SetLanguage SetLanguageParams { get { return s_params_SetLanguage; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SetLanguage
        {
            public readonly string l = "l";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string AnnounceBlock = "AnnounceBlock";
                public readonly string ChildrenBlock = "ChildrenBlock";
                public readonly string Gallery = "Gallery";
                public readonly string GalleryFiles = "GalleryFiles";
                public readonly string Index = "Index";
                public readonly string LeftMenu = "LeftMenu";
                public readonly string MainSlider = "MainSlider";
                public readonly string NewsBlock = "NewsBlock";
                public readonly string Page = "Page";
                public readonly string SiteMap = "SiteMap";
                public readonly string TopMenu = "TopMenu";
            }
            public readonly string AnnounceBlock = "~/Views/Default/AnnounceBlock.cshtml";
            public readonly string ChildrenBlock = "~/Views/Default/ChildrenBlock.cshtml";
            public readonly string Gallery = "~/Views/Default/Gallery.cshtml";
            public readonly string GalleryFiles = "~/Views/Default/GalleryFiles.cshtml";
            public readonly string Index = "~/Views/Default/Index.cshtml";
            public readonly string LeftMenu = "~/Views/Default/LeftMenu.cshtml";
            public readonly string MainSlider = "~/Views/Default/MainSlider.cshtml";
            public readonly string NewsBlock = "~/Views/Default/NewsBlock.cshtml";
            public readonly string Page = "~/Views/Default/Page.cshtml";
            public readonly string SiteMap = "~/Views/Default/SiteMap.cshtml";
            public readonly string TopMenu = "~/Views/Default/TopMenu.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_DefaultController : MedIn.Web.Controllers.DefaultController
    {
        public T4MVC_DefaultController() : base(Dummy.Instance) { }

        [NonAction]
        partial void IndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Index()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            IndexOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void PageOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Page()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Page);
            PageOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void MainSliderOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult MainSlider()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.MainSlider);
            MainSliderOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void GalleryOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int id);

        [NonAction]
        public override System.Web.Mvc.ActionResult Gallery(int id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Gallery);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            GalleryOverride(callInfo, id);
            return callInfo;
        }

        [NonAction]
        partial void GalleryFilesOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int id, int? count);

        [NonAction]
        public override System.Web.Mvc.ActionResult GalleryFiles(int id, int? count)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GalleryFiles);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "count", count);
            GalleryFilesOverride(callInfo, id, count);
            return callInfo;
        }

        [NonAction]
        partial void TopMenuOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult TopMenu()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.TopMenu);
            TopMenuOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void LeftMenuOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult LeftMenu()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LeftMenu);
            LeftMenuOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void NewsBlockOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult NewsBlock()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.NewsBlock);
            NewsBlockOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ChildrenBlockOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string alias, string imgUrl);

        [NonAction]
        public override System.Web.Mvc.ActionResult ChildrenBlock(string alias, string imgUrl)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ChildrenBlock);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "alias", alias);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "imgUrl", imgUrl);
            ChildrenBlockOverride(callInfo, alias, imgUrl);
            return callInfo;
        }

        [NonAction]
        partial void SetLanguageOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string l);

        [NonAction]
        public override System.Web.Mvc.ActionResult SetLanguage(string l)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SetLanguage);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "l", l);
            SetLanguageOverride(callInfo, l);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009