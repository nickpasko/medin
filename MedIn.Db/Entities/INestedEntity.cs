using System.Data.Objects.DataClasses;

namespace MedIn.Db.Entities
{
    public interface INestedEntity<TEntity> : IPlainTreeItem
        where TEntity : class, IEntity
    {
        EntityCollection<TEntity> Children { get; set; }
    }
}
