using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Web.Mvc;

namespace MedIn.Libs
{
	public sealed class DynamicViewDataDictionary : DynamicObject
	{
		private readonly Func<ViewDataDictionary> _viewDataThunk;

		public DynamicViewDataDictionary(Func<ViewDataDictionary> viewDataThunk)
		{
			_viewDataThunk = viewDataThunk;
		}

		private ViewDataDictionary ViewData
		{
			get
			{
				ViewDataDictionary viewData = _viewDataThunk();
				Debug.Assert(viewData != null);
				return viewData;
			}
		}

		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return ViewData.Keys;
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			result = ViewData[binder.Name];
			return true;
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			ViewData[binder.Name] = value;
			return true;
		}
	}
}