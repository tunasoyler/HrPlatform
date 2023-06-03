using System.ComponentModel.DataAnnotations;

namespace Bilgeadam.HrPlatform.Entities.DTOs
{
    public class CompanyUpdateDtos
    {
        
        public int Id { get; set; }
        [Required(ErrorMessage = "Company name can not be null!")]
        [StringLength(50,ErrorMessage = "Company name can be up to 50 characters!")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Company title can not be null!")]
        [StringLength(50, ErrorMessage = "Company title can be up to 50 characters!")]
        public string CompanyTitle { get; set; }
        public byte[]? Logo { get; set; }
        
        [StringLength(10,MinimumLength = 10,ErrorMessage ="Phone number must contain 10 digit characters!")]
        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Home Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        [EmailAddress(ErrorMessage = "This e-mail is not valid.")]
        [Required(ErrorMessage = "Email can not be null!")]
        public string Email { get; set; }
        public bool Status { get; set; }
    }
}
