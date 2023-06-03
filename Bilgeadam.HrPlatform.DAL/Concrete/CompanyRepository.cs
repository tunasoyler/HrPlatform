using Bilgeadam.HrPlatform.DAL.Abstract;
using Bilgeadam.HrPlatform.DAL.Context;
using Bilgeadam.HrPlatform.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bilgeadam.HrPlatform.DAL.Concrete
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
       private readonly UserManager<AppUser> _userManager;

        public CompanyRepository(DbContext _context,UserManager<AppUser> userManager) : base(_context)
        {
            //var companyManager = _context.CompanyManagers
            //        .Include(cm => cm.Company)
            //        .SingleOrDefault(cm => cm.Id == user.Id);
            _userManager= userManager;
        }
        private HrPlatformDB dbContext
        {
            get { return context as HrPlatformDB; }
        }
        public  IEnumerable<AppUser> GetCompanyManagerWithCompany()
        {
            var companyManagers =_userManager.GetUsersInRoleAsync("CompanyManager").Result;
            List<AppUser> appusers = new List<AppUser>();
            foreach (var item in companyManagers)
            {
                var user = context.Set<AppUser>().Where(x => x.Id== item.Id).Include(x => x.Company).FirstOrDefault();
                appusers.Add(user);
            }
            return appusers;
        }
    }

}
