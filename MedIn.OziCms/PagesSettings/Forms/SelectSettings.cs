using System.Collections.Generic;

namespace MedIn.OziCms.PagesSettings.Forms
{
	public class SelectSettings : FieldSettings
	{
		public string Data { get; set; }
		public string OptionTitle { get; set; }
		public string OptionValue { get; set; }
		public List<OptionSettings> Options { get; set; }
		public bool Levels { get; set; }
		public override string Control { get { return "ozi_Select"; } }
		public bool Multiple { get; set; }
		
		public bool Editable { get; set; }

		public string Reference { get; set; }
		public string DependsOn { get; set; }

		public string TypeName { get; set; }
		public string PropertyName { get; set; }
		public string MethodName { get; set; }
		public int Height { get; set; }

		protected override string GetCurrentPropertyName()
		{
			return Reference ?? Name;
		}
	}
}
