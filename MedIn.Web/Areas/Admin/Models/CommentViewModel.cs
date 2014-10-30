using System.ComponentModel.DataAnnotations;

namespace MedIn.Web.Areas.Admin.Models
{
	public class CommentViewModel
	{
		public int Id { get; set; }

		[DataType(DataType.MultilineText)]
		public string Text { get; set; }
	}
}