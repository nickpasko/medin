using System.Data.Objects;
using System.Diagnostics;

namespace MedIn.Db.Infrastructure.Implementation
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly IDatabaseFactory _databaseFactory;
		private ObjectContext _dataContext;

		[DebuggerStepThrough]
		public UnitOfWork(IDatabaseFactory databaseFactory)
		{
			_databaseFactory = databaseFactory;
		}

		protected ObjectContext DataContext
		{
			get { return _dataContext ?? (_dataContext = _databaseFactory.Get()); }
		}

		public void Commit()
		{
			DataContext.SaveChanges();
		}
	}
}
