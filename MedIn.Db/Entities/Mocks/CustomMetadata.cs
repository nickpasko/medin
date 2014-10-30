namespace MedIn.Db.Entities.Mocks
{
	public class CustomMetadata : IMetadataEntity
	{
		public int Id { get; set; }
		public string MetaDescription { get; set; }
		public string MetaKeywords { get; set; }
		public string MetaTitle { get; set; }
	}
}
