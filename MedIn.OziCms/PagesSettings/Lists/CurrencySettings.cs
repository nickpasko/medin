using MedIn.OziCms.Mvc;

namespace MedIn.OziCms.PagesSettings.Lists
{
	public class CurrencySettings : ColSettings
	{
		//TODO не реализовано
		public string Format { get; set; }
		public override string Control { get { return ControlsNames.Currency; } }
	}
}
