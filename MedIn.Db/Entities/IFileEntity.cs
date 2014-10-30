using System;

namespace MedIn.Db.Entities
{
	public interface IFileEntity : IVisibleEntity, ISortableEntity
	{
		string Name { get; set; }
		string SourceName { get; set; }
		string Alt { get; set; }
		string Title { get; set; }
		string Description { get; set; }
		DateTime Date { get; set; }
		long? Size { get; set; }
	}
}
