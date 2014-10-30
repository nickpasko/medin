using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using MedIn.Db.Entities;
using MedIn.Libs;
using MedIn.OziCms.PagesSettings;
using MedIn.OziCms.PagesSettings.Forms;

namespace MedIn.OziCms.Mvc
{
	public class AdminWebContext
	{
		public AdminWebContext()
		{
			
		}

		private Settings _settings;
		private string _returnUrl;

		public string ReturnUrl
		{
			get { return _returnUrl ?? HttpContext.Current.Request["returnUrl"] ?? HttpContext.Current.Request["ozi_backlink"]; }
			set { _returnUrl = value; }
		}

		public int? PrevId { get; set; }
		public int? NextId { get; set; }

		public bool IsCreate { get; set; }

		public string EditViewName { get; set; }

		public Settings GetSettings(Type controllerType = null)
		{
			if (controllerType == null)
				return _settings;
			return _settings ?? (_settings = new Settings(GetSettingsName(controllerType)));
		}

		private string GetSettingsName(Type controllerType)
		{
			Debug.Assert(controllerType.BaseType != null);
			return controllerType.BaseType.GetGenericArguments()[0].Name;
		}

		private ViewDataDictionary _viewData;
		private DynamicViewDataDictionary _dynamicViewData;

		public dynamic ViewBag
		{
			get { return _dynamicViewData ?? (_dynamicViewData = new DynamicViewDataDictionary(() => ViewData)); }
		}

		protected virtual void SetViewData(ViewDataDictionary viewData)
		{
			_viewData = viewData;
		}

		public ViewDataDictionary ViewData
		{
			get
			{
				if (_viewData == null)
				{
					SetViewData(new ViewDataDictionary());
				}
				return _viewData;
			}
			set { SetViewData(value); }
		}

		public FieldSettings FieldSettings { get; set; }

		public TabsSettings CurrentTab { get; set; }

		public string HtmlPageTitle { get; set; }

		public IEntity Model { get; set; }

		public Type ModelType { get; set; }
	}
}