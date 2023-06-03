using Bilgeadam.HrPlatform.Entities.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
//using System.Web.WebPages.Html;

namespace Bilgeadam.HrPlatform.Entities.DTOs
{
    public class CompanyManagerDto:AppUser
    {

        public byte[]? ProfilePhoto { get; set; }
        [Required(ErrorMessage = "Firstname is required")]
        public string FirstName
        { get; set;}
        public string? SecondName
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Lastname is required")]
        public string LastName
        {
            get;
            set;
        }
        public string? SecondLastName
        {
            get;
            set;
        }
        [StringLength(11, ErrorMessage = "Phone number must be 11 digits")]
        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "BirthDate is required")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "BirthPlace is required")]
        public string BirthPlace { get; set; }
        [Required(ErrorMessage = "Tc Number is required")]
        [StringLength(11,ErrorMessage ="Tc Number must be 11 digits")]
        public string TCNo { get; set; }
        [Required(ErrorMessage = "Profession is required")]
        public string? Profession { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Company Name is required")]
        public int? CompanyId { get; set; }
        public string Password { get; set; }
        public List<SelectListItem>? CompanyForDropDown { get; set; }

        public Company? Company { get; set; }
    }
}
