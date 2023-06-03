using Bilgeadam.HrPlatform.DAL.Abstract;
using Bilgeadam.HrPlatform.DAL.Context;
using Bilgeadam.HrPlatform.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilgeadam.HrPlatform.DAL.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HrPlatformDB context;
        private UserManager<AppUser> userManager;
        private CompanyRepository companyRepository;
        private CompanyManagerRepository companyManagerRepository;
        private AdvanceRepository advanceRepository;

        public UnitOfWork(HrPlatformDB _context, UserManager<AppUser> userManager)
        {
            context = _context;
            this.userManager = userManager;
        }

        public UserManager<AppUser> UserManager { get; }

        public ICompanyRepository Company => companyRepository=companyRepository ?? new CompanyRepository(context,userManager);
        public ICompanyManagerRepository CompanyManager => companyManagerRepository ?? new CompanyManagerRepository(context, userManager);
        public IAdvanceRepository Advance => advanceRepository ?? new AdvanceRepository(context, userManager);

        public  IQueryable<Company>  QueryableCompanyList =>  context.Companies.ToList().AsQueryable();




        //public UserManager<AppUser> SiteManager => userManager = userManager ?? new UserManager<AppUser>(context);

        public async Task<int> CommitAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
           context.Dispose();
        }
    }
}
