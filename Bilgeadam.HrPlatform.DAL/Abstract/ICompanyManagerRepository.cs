using Bilgeadam.HrPlatform.DAL.Concrete;
using Bilgeadam.HrPlatform.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilgeadam.HrPlatform.DAL.Abstract
{
    public interface ICompanyManagerRepository:IRepository<CompanyManager>
    {
        AppUser GetUserWithCompany(string id);
        
    }
}
