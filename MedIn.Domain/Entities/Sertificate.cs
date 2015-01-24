using MedIn.Db.Entities;

namespace MedIn.Domain.Entities
{
    public partial class Sertificate : IVisibleEntity, ISortableEntity
    {
        public override string ToString()
        {
            return Name;
        }
    }
}
