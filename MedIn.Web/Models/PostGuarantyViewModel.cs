using System.ComponentModel.DataAnnotations;
using MedIn.Libs.Validation;

namespace MedIn.Web.Models
{
    public class PostGuarantyViewModel
    {
        [Required(ErrorMessage = "*")]
        [MaxLength(128, ErrorMessage = "Максимальная длина поля не должна превышать символов")]
        public string Organization { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(128, ErrorMessage = "Максимальная длина поля не должна превышать символов")]
        public string Persone { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(128, ErrorMessage = "Максимальная длина поля не должна превышать символов")]
        public string Phone { get; set; }

        [Email(ErrorMessage = "Проверьте правильность адреса эл. почты")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(128, ErrorMessage = "Максимальная длина поля не должна превышать символов")] 
        public string Email { get; set; }

        [MaxLength(128, ErrorMessage = "Максимальная длина поля не должна превышать символов")]
        public string ProductName { get; set; }

        public int Year { get; set; }
        public string Description { get; set; }
    }
}