using Bilgeadam.HrPlatform.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilgeadam.HrPlatform.DAL.Abstract
{
    public interface IAdvanceRepository :IRepository<Advance>
    {
        IEnumerable<Advance> GetAdvancesWithEmployee(string id);
        IEnumerable<Advance> GetAdvancesWithCompany(string id);
    }
}
