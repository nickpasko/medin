using MedIn.Db.Entities;

namespace MedIn.Domain.Entities
{
    partial class File : IFileEntity
    {
        public override string ToString()
        {
            return Name;
        }
    }
}
