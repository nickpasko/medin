using System;
using System.ComponentModel.DataAnnotations;

namespace MedIn.Web.Models
{
    public class RegisterViewModel
    {
        //[LRequired(ErrorMessagePropertyName = "RequiredField")]
        public string UserName { get; set; }

        //[LRequired(ErrorMessagePropertyName = "RequiredField")]
        //[LEmail(ErrorMessagePropertyName = "EmailError")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        //[LRequired(ErrorMessagePropertyName = "RequiredField")]
        public string Password { get; set; }

        //[LRequired(ErrorMessagePropertyName = "RequiredField")]
        //[LCompare("Password", ErrorMessagePropertyName = "ComparePasswordError")]
        public string ConfirmPassword { get; set; }

        public Guid Key { get; set; }

    }
}