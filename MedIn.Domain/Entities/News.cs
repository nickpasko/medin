using MedIn.Db.Entities;

namespace MedIn.Domain.Entities
{
    partial class News : IVisibleEntity, IMetadataEntity
	{
        public override string ToString()
        {
            return Name;
        }
	}
}
