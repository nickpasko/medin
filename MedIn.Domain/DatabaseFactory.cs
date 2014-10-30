using System.Data.Objects;
using MedIn.Db.Infrastructure;
using MedIn.Domain.Entities;

namespace MedIn.Domain
{
	public class DatabaseFactory : IDatabaseFactory
	{
		public DatabaseFactory(DataModelContext db)
		{
			_db = db;
		}

		private DataModelContext _db;

		public ObjectContext Get()
		{
			return _db;
		}
	}
}
