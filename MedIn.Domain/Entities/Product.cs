using MedIn.Db.Entities;

namespace MedIn.Domain.Entities
{
    public partial class Product:ISortableEntity, IVisibleEntity, IHaveAliasEntity, IMetadataEntity
    {
        public override string ToString()
        {
            return Name;
        }

        public string Url
        {
            get { return string.Format("{0}/{1}", Category.Url, Alias); }
        }
    }
}
