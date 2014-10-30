namespace MedIn.Db.Infrastructure
{
	public class ValidationResult
	{
		public ValidationResult()
		{
		}

		public ValidationResult(string memberName, string message)
		{
			MemberName = memberName;
			Message = message;
		}

		public ValidationResult(string message)
		{
			Message = message;
		}

		public string MemberName { get; set; }

		public string Message { get; set; }
	}
}
