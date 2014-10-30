namespace MedIn.Db.Entities
{
    public interface IVisibleEntity : IEntity
    {
        bool Visibility { get; set; }
    }
}
