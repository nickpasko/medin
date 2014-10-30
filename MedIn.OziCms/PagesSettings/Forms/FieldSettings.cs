namespace MedIn.OziCms.PagesSettings.Forms
{

	public class FieldSettings
    {
        public virtual string Control { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }
		public string Condition { get; set; }
        public bool Disabled { get; set; }
        public bool SkipTitle { get; set; }
        public bool Readonly { get; set; }
		public bool PreValue { get; set; }
		public bool Localizable { get; set; }
		public CollectionSettings Parent { get; set; } // используется для вложенных коллекций

		public virtual string GetFullPropertyName()
		{
			return Parent == null ? GetCurrentPropertyName() : string.Concat(Parent.GetFullPropertyName(), ".", GetCurrentPropertyName());
		}

		public virtual string GetFullPropertyId()
		{
			return GetFullPropertyName().Replace('.', '_');
		}

		protected virtual string GetCurrentPropertyName()
		{
			return Name;
		}

		//public string GetFullPropertyPath(int indexes)
		//{
		//	return Parent == null ? Name : string.Concat(Parent.GetFullPropertyName(), ".", Name);
		//}
    }
}