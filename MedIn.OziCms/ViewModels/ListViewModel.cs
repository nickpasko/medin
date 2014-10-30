using System.Collections.Generic;
using MedIn.OziCms.Infrastructure;
using MedIn.OziCms.PagesSettings;

namespace MedIn.OziCms.ViewModels
{
    public class ListViewModel
    {
        public Settings Settings { get;set; }
        public IEnumerable<object> ListData { get; set; }
    	public FilterParameters FilterParameters { get; set; }
    }
}