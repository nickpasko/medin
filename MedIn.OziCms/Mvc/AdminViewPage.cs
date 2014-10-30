using System.Web.Mvc;

namespace MedIn.OziCms.Mvc
{
	public abstract class AdminViewPage<TModel> : DotOrgViewPage<TModel>
	{
		public AdminWebContext WebContext { get { return DependencyResolver.Current.GetService<AdminWebContext>(); } }
		protected override IDotOrgWebContext DotOrgWebContext
		{
			get { return null; }
		}

		public class Actions
		{
			public static string SendMail { get { return "SendMail"; } }
			public static string Index { get { return "Index"; } }
			public static string Edit { get { return "Edit"; } }
			public static string UploadFile { get { return "UploadFile"; } }
			public static string UploadFileCollectionItem { get { return "UploadFileCollectionItem"; } }
			public static string SaveFileData { get { return "SaveFileData"; } }
			public static string DeleteFile { get { return "DeleteFile"; } }
			public static string SaveOrder { get { return "SaveOrder"; } }
		}
	}

	public abstract class AdminViewPage : AdminViewPage<object>
	{
	}
}