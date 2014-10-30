using MedIn.Db.Entities;

namespace MedIn.Domain.Entities
{
	partial class UserEntity : IEntity
	{
		public override string ToString()
		{
			return DisplayName ?? UserName;
		}
	}
}
