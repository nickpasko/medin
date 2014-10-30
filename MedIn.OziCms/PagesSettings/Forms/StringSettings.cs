namespace MedIn.OziCms.PagesSettings.Forms
{
	public class StringSettings : FieldSettings
	{
		public int Maxlength { get; set; }
		public override string Control { get { return "ozi_String"; } }
	}
}
