using System;

namespace MedIn.Db.Entities.Mocks
{
	public class CustomFileEntity : IFileEntity
	{
		public int Id { get; set; }
		public bool Visibility { get; set; }
		public int Sort { get; set; }
		public string Name { get; set; }
		public string SourceName { get; set; }
		public string Alt { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime Date { get; set; }
		public long? Size { get; set; }
		public string PropName { get; set; }
		public int ObjId { get; set; }
	}
}
