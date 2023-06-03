using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilgeadam.HrPlatform.Entities.DTOs
{
    public class CompanyManagerUpdateDto
    {
        public string  Id { get; set; }
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must contain 10 digit characters!")]
        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Home Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string  PhoneNumber { get; set; }
        public byte[]? ProfilePhoto { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public string? Address { get; set; }
        [Required(ErrorMessage = "Gender is not null")]
        public string Gender { get; set; }

        [StringLength(5, MinimumLength = 5, ErrorMessage = "Wage must contain 5 digit characters!")]
        public string Wage { get; set; }
    }
}
