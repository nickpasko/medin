using System.ComponentModel.DataAnnotations;

namespace MedIn.Web.Models
{
	public class FeedbackViewModel
	{
        //[LRequired(ErrorMessagePropertyName = "RequiredField")]
		public string Username { get; set; }

        //[LRequired(ErrorMessagePropertyName = "RequiredField")]
        //[LEmail(ErrorMessagePropertyName = "EmailError")]
		public string Email { get; set; }

		[DataType(DataType.MultilineText)]
        //[LRequired(ErrorMessagePropertyName = "RequiredField")]
		public string Message { get; set; }

		public int Id { get; set; }
		public string ToastrMessage { get; set; }
		public bool Success { get; set; }
	}
}