namespace MedIn.OziCms.PagesSettings.Forms
{
	public class DateSettings : FieldSettings
	{
		public string Format { get; set; }
		public override string Control { get { return "ozi_Date"; } }
	}
}
