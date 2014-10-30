namespace MedIn.OziCms.PagesSettings.Forms
{
	public class TextareaSettings : FieldSettings
	{
		public int Rows { get; set; }
		public override string Control { get { return "ozi_Textarea"; } }
	}
}
