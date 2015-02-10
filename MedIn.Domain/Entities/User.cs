using MedIn.Db.Entities;

namespace MedIn.Domain.Entities
{
	partial class User : IEntity
	{
		public override string ToString()
		{
			return DisplayName ?? UserName;
		}
	}
}
