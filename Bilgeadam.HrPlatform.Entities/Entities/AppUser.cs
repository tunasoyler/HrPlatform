using Bilgeadam.HrPlatform.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace Bilgeadam.HrPlatform.Entities.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public AppUser()
        {
            Advances = new List<Advance>();
        }

        public static string ConvertTurkishCharacters(string input)
        {
            string normalized = input.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string GenerateEmail(string name, string surname)
        {
            if(name!=null&&surname!=null)
            {
                string convertedName = ConvertTurkishCharacters(name.ToLower());
                string convertedSurname = ConvertTurkishCharacters(surname.ToLower());

                string email = $"{convertedName}.{convertedSurname}@bilgeadamboost.com";
                return email;
            }
            return string.Empty;
            
        }
     
        public override string? Email
        {
            get
            {
                return GenerateEmail(firstName, lastName);
            }

        }
        public override string? UserName { get { return GenerateEmail(firstName, lastName); } }
        public DateTime CreatedDate { get; set; } = DateTime.Today;
        public DateTime UpdatedDate { get; set; } = DateTime.Today;
        public byte[]? ProfilePhoto { get; set; }
        private string firstName;
        public string FirstName 
        {
            get { return firstName; }
            set { firstName = value.ToLower(); }
        }
        private string secondName;
        public string? SecondName
        {
            get { return secondName; }
            set { secondName = value.ToLower(); }
        }
        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set { lastName = value.ToLower(); }
        }
        private string secondLastName;
        public string? SecondLastName
        {
            get { return secondLastName; }
            set { secondLastName = value.ToLower(); }
        }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string TCNo { get; set; }
        public DateTime DateOfRecruitment { get; set; } = DateTime.Today;
        public DateTime? ContractEndDate { get; set; } = DateTime.Today;
        public string? Profession { get; set; }
        public string? Department { get; set; }
        public string? ImagePath { get; set; }
        public string Gender { get; set; }
        //[RegularExpression("^[0-9]+$")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must contain 10 digit characters!")]
        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Home Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number,phone number must be numbers only")]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber=value; }
        public string? Address { get; set; }
        public Company? Company { get; set; }
        public int? CompanyId { get; set; }
        public bool Status { get; set; } = true;
        public List<Advance>? Advances { get; set; }
        public double? Wage { get { return Wage; } set { Wage = Convert.ToDouble(value); } }

    }
}
