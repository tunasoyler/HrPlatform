using Bilgeadam.HrPlatform.BLL.Abstract;
using Bilgeadam.HrPlatform.BLL.Concrete;
using Bilgeadam.HrPlatform.BLL.MailConfigurationService;
using Bilgeadam.HrPlatform.DAL.Abstract;
using Bilgeadam.HrPlatform.DAL.Concrete;
using Bilgeadam.HrPlatform.DAL.Context;
using Bilgeadam.HrPlatform.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bilgeadam.HrPlatform.BLL.DependencyResolver.Microsoft
{
    public static class DependencyExtension
    {
        public static void AddDependcy(this IServiceCollection services)
        {
            services.AddDbContext<HrPlatformDB>(opt =>
            {

                //opt.UseSqlServer("Server=YUSATOSUN\\SQLEXPRESS;Database=SitemanagerDb;Trusted_Connection=True");
                //opt.UseSqlServer("Server=DENIZ;Database=SitemanagerDb;Trusted_Connection=True");
                //opt.UseSqlServer("Server=DESKTOP-6N0TFDT;Database=SitemanagerDb;Trusted_Connection=True;");
                //opt.UseSqlServer("Data Source=DESKTOP-BVE8G4S;Database=HRPlatform;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
                //opt.UseSqlServer("Server=tcp:hrproject.database.windows.net, 1433; Initial Catalog = hrproject; Persist Security Info=False; User ID = HrProject; Password=Huseyin.97; MultipleActiveResultSets=False; Encrypt=True; TrustServerCertificate=False;");
                opt.UseSqlServer("Server=tcp:hrproject.database.windows.net,1433;Initial Catalog=hrproject;Persist Security Info=False;User ID=HrProject;Password=Huseyin.97;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=3000;");


            });

            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.SignIn.RequireConfirmedEmail = false;

            }).AddEntityFrameworkStores<HrPlatformDB>();
          
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICompanyManagerServices, CompanyManagerServices>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IAdvanceService, AdvanceService>();

        }
    }
}
