namespace MedIn.OziCms.PagesSettings.Forms
{
	public class CurrencySettings : FieldSettings
	{
		public string Format { get; set; }
		public override string Control { get { return "ozi_Currency"; } }
	}
}
