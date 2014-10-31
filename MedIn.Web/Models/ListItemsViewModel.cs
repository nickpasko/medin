using System.Collections.Generic;
using MedIn.OziCms.ViewModels;

namespace MedIn.Web.Models
{
	public class ListItemsViewModel<T>
	{
		public IEnumerable<T> Items { get; set; }
		public PagerViewModel Pager { get; set; }
	}
}
