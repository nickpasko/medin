using MedIn.OziCms.Mvc;

namespace MedIn.OziCms.PagesSettings.Lists
{
	public class ImageSettings : ColSettings
	{
		public string ImageWidth { get; set; }
		public override string Control { get { return ControlsNames.Image; } }
	}
}
