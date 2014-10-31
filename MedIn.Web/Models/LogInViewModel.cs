namespace MedIn.Web.Models
{
    public class LogInViewModel
    {
        //[LRequired(ErrorMessagePropertyName = "RequiredField")]
        public string LogIn { get; set; }

        //[LRequired(ErrorMessagePropertyName = "RequiredField")]
        public string Password { get; set; }

        public bool Remember { get; set; }
    }
}