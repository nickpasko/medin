namespace MedIn.Db.Entities
{
	public interface IPlainTreeItem : IEntity
	{
		int Level { get; set; }
		int? ParentId { get; set; }
		bool HasChilds { get; set; }
	}
}