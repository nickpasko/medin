namespace MedIn.Db.Entities
{
	public interface IMetadataEntity : IEntity
	{
		string MetaDescription { get; set; }
		string MetaKeywords { get; set; }
		string MetaTitle { get; set; }
	}
}
