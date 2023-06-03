using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilgeadam.HrPlatform.Entities.Entities
{
        public class AppRole : IdentityRole<Guid>
        {
            public DateTime CreatedTime {  get; set; }
        }    
}
