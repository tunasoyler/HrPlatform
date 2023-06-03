using Bilgeadam.HrPlatform.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilgeadam.HrPlatform.BLL.Abstract
{
    public interface IEmployeeService
    {
        Task<IEnumerable<AppUser>> GetEmployeesWithCompany(int companyid);
    }
}
