using System.Collections.Generic;
using System.Data.Objects;

namespace MedIn.Db.Infrastructure
{
    public interface IRepository<TEntity>
    {
		ObjectContext Context { get; }
        IEnumerable<TEntity> All { get; }
        TEntity GetById(int id);
        TEntity Create();
        void Add(TEntity entity);
        void Delete(TEntity entity);
        void DeleteById(int id);
    }
}
