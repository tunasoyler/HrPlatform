using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilgeadam.HrPlatform.Entities.DTOs
{
    public class SiteManagerUpdateDTO:BaseDTO
    {
        public byte[]? ProfilePhoto { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set;}
    }
}
