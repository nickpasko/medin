namespace MedIn.OziCms.PagesSettings.Forms
{
	public class CodeSettings : FieldSettings
	{
		public string Type { get; set; }
		public override string Control { get { return "ozi_Code"; } }
	}
}
