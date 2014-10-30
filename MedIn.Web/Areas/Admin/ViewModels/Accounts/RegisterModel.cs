using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MedIn.Libs.Validation;

namespace MedIn.Web.Areas.Admin.ViewModels.Accounts
{
	[PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "The password and confirmation password do not match.")]
	public class RegisterModel
	{
		[Required]
		[DisplayName("User name")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[DisplayName("Email address")]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[DisplayName("Password")]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[DisplayName("Confirm password")]
		public string ConfirmPassword { get; set; }
	}
}