using MedIn.Db.Entities;

namespace MedIn.Domain.Entities
{
    partial class Question : IVisibleEntity
    {
        public override string ToString()
        {
            return UserName;
        }
    }
}
