using System.Text.RegularExpressions;

namespace MedIn.Libs.Validation
{
	public static class EmailValidator
	{
		public const string EmailPattern = "^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$";
		private static readonly Regex EmailRegex = new Regex(EmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

		public static bool ValidateEmail(string email)
		{
			return EmailRegex.IsMatch(email);
		}
	}
}
