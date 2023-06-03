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
    public class CompanyManagerRepository : Repository<CompanyManager>, ICompanyManagerRepository
    {
        private readonly UserManager<AppUser> _userManager;
        public CompanyManagerRepository(DbContext dbContext, UserManager<AppUser> userManager) : base(dbContext)
        {

        }
        private HrPlatformDB DbContext
        {
            get {  return context as HrPlatformDB; }
        }

        public AppUser GetUserWithCompany(string id)
        {
            var user = context.Set<AppUser>().Where(x => x.Id.ToString()== id).Include(x => x.Company).FirstOrDefault();
            return user;
        }
    }
}
