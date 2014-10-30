namespace MedIn.OziCms.Mvc
{
	public class Constants
	{
		public static readonly string AuthenticationType = "DotOrgForms";

		public static readonly string IndexView = "Index";
		public static readonly string EditView = "Edit";
		public static readonly string CreateView = "Create";
		public static readonly string DetailsView = "Details";
		public static readonly string DeleteAction = "Delete";
		public static readonly string SetVisibilityAction = "SetVisibility";
		public static readonly string SortListAction = "SortList";
	}

	public class FieldsNames
	{
		public const string Alias = "Alias";
		public const string SaveButton = "_save";
		public const string CloneButton = "_clone";
		public const string PrototypeSelect = "_prototype";
		public const string SortListPrefix = "SortListItems";
		public const string FileInputPrefix = "image-file";
		public const string FileDeleteCheckboxName = "delete-file";
		public const string FileDescriptionName = "description-file";
		public const string FileTitleName = "title-file";
		public const string FileAltTextName = "alt-file";
		public const string FileIdName = "id-file";
		public const string FileDeletePostixName = "_delete";
	}

	public class ControlsNames
	{
		public const string String = "ozi_String";
		public const string Filter = "ozi_Filter";
		public const string Date = "ozi_Date";
		public const string Checkbox = "ozi_Checkbox";
		public const string Create = "create";
		public const string Localize = "localize";
		public const string Currency = "ozi_Currency";
		public const string Image = "ozi_Image";
		public const string Sort = "sort";
		public const string Visibility = "visibility";
		//public const string = "";
	}
}