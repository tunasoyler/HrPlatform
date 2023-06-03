using Bilgeadam.HrPlatform.DAL.Abstract;
using Bilgeadam.HrPlatform.DAL.Context;
using Bilgeadam.HrPlatform.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilgeadam.HrPlatform.DAL.Concrete
{
    public class AdvanceRepository : Repository<Advance>, IAdvanceRepository
    {
        private readonly UserManager<AppUser> _userManager;

        public AdvanceRepository(DbContext dbContext, UserManager<AppUser> userManager) : base(dbContext)
        {

        }
        private HrPlatformDB dbContext
        {
            get { return context as HrPlatformDB; }
        }

        public IEnumerable<Advance> GetAdvancesWithEmployee(string id)
        {
            var advances = context.Set<Advance>().Where(x => x.EmployeeId.ToString() == id).Include(x => x.Employee);
            return advances;
        }
        public IEnumerable<Advance> GetAdvancesWithCompany(string id)
        {
            var advances = context.Set<Advance>().Where(x => x.Employee.CompanyId.ToString() == id).Include(x => x.Employee);
            return advances;
        }

    }
}
