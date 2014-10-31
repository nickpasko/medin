namespace MedIn.Web.Models
{
	public class ItemViewModel<T>
	{
		public T Item { get; set; }
		public int? ItemPage { get; set; }
		public string CategoryAlias { get; set; }
	}
}