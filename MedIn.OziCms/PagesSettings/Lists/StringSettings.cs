using MedIn.OziCms.Mvc;

namespace MedIn.OziCms.PagesSettings.Lists
{
	public class StringSettings : ColSettings
	{
		//TODO не реализовано
		public int Maxlength { get; set; }
		public override string Control { get { return ControlsNames.String; } }
	}
}
