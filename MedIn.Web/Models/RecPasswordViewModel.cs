using System;

namespace MedIn.Web.Models
{
    public class RecPasswordViewModel
    {
        //[LRequired(ErrorMessagePropertyName = "RequiredField")]
        public string NewPassword { get; set; }

        //[LRequired(ErrorMessagePropertyName = "RequiredField")]
        //[LCompare("NewPassword", ErrorMessagePropertyName = "ComparePasswordError")]
        public string ConfirmPassword { get; set; }

        public Guid? Key { get; set; }
    }
}