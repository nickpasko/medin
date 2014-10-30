using MedIn.Db.Entities;

namespace MedIn.Domain.Entities
{
    partial class Article : IVisibleEntity, ISortableEntity, IHaveAliasEntity, IMetadataEntity
    {
        public override string ToString()
        {
            return Name;
        }
    }
}
