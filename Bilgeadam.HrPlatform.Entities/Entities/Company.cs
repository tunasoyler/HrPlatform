using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Bilgeadam.HrPlatform.Entities.Entities
{
    public class Company:BaseEntity
    {
        public string CompanyName { get; set; }
        public string CompanyTitle { get; set; }
        
        public string? MersisNumber { get; set; }
        public string? TaxIdentificationNumber { get; set; }
        public string? TaxOffice { get; set; }
        public byte[]? Logo { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Not a valid phone number,phone number must be numbers only")]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int? EmployeeCount { get; set; }
        public DateTime? Establishment { get; set; }
        public DateTime? ContactStartDate { get; set; }
        public DateTime? ContactEndDate { get; set; }
        public bool IsValidContractDates()
        {
            if (ContactEndDate<DateTime.Now)
            {
                return false;
            }
            return  ContactStartDate < ContactEndDate;
        }
      
        public List<AppUser>? AppUsers { get; set; }
        public bool Status { get; set; }
    }
}
