using System.Web.Mvc;
using MedIn.Db.Entities;

namespace MedIn.OziCms.Mvc
{
	public interface IDotOrgWebContext
	{
		IMetadataEntity Metadata { get; set; }
		dynamic ViewBag { get; }
		ViewDataDictionary ViewData { get; set; }
	}
}