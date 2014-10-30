using System.Data.Objects.DataClasses;
using MedIn.Db.Entities;

namespace MedIn.Domain.Entities
{
	partial class Location: IVisibleEntity, ISortableEntity, IHaveAliasEntity, IMetadataEntity, INestedEntity<Location>
	{
        public int Level { get; set; }
        public bool HasChilds { get; set; }
        public EntityCollection<Location> Children
        {
            get { return Locations1; }
            set { Locations1 = value; }
        }

        public Location Parent
        {
            get { return Location1; }
            set { Location1 = value; }
        }

		public override string ToString()
		{
			return Name;
		}

		public static Location Empty
		{
			get { return new Location(); }
		}

		public Location Root
		{
			get
			{
				return Parent == null ? this : Parent.Root;
			}
		}

		public bool InPath(string alias)
		{
			if (Alias == alias)
				return true;
			if (ParentId == null)
				return false;
			return Parent.InPath(alias);
		}
	}
}
