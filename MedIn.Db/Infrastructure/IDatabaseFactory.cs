using System.Data.Objects;

namespace MedIn.Db.Infrastructure
{
	public interface IDatabaseFactory
	{
		ObjectContext Get();
	}
}
