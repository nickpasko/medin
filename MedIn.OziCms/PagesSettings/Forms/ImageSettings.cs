namespace MedIn.OziCms.PagesSettings.Forms
{
	public class ImageSettings : FieldSettings
	{
		public override string Control { get { return "ozi_image"; } }
		public int Width { get; set; }
		public int Height { get; set; }
		public string Alt { get; set; }
		public bool SimpleForm { get; set; }
		public bool Proportional { get; set; }
		public string Reference { get; set; }
	}

	//public class ImageSettings
	//{
	//    public int MaxWidth { get; set; }
	//    public int MaxHeight { get; set; }
	//    public string Prefix { get; set; }
	//    public bool ConstProp { get; set; }
	//}

}
