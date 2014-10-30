namespace MedIn.Db.Entities
{
    public interface ISortableEntity : Db.Entities.IEntity
    {
        int Sort { get; set; }
    }
}
