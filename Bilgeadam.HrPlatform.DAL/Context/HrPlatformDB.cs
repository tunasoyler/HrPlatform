using Bilgeadam.HrPlatform.Entities.Entities;
using Bilgeadam.HrPlatform.Entities.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bilgeadam.HrPlatform.DAL.Context
{
    public class HrPlatformDB : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public DbSet<AppUser> SiteManagers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public HrPlatformDB(DbContextOptions<HrPlatformDB> option) : base(option)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            string filePath = "Content/assets/images/faces/face1.jpg";
            byte[] imageData = System.IO.File.ReadAllBytes(filePath);
            builder.Entity<AppUser>().HasData(new AppUser
            {
                Id = Guid.NewGuid(),
                ProfilePhoto = imageData,
                FirstName = "yusa",
                LastName = "tosun",
                BirthDate = DateTime.Now.AddYears(-28),
                BirthPlace = "Zonguldak",
                TCNo = "58777493212",
                DateOfRecruitment = DateTime.Now,
                NormalizedEmail="YUSA.TOSUN@BILGEADAM.COM",
                PhoneNumber = "05643212348",
                Gender = "Male",
                PasswordHash = "AQAAAAEAACcQAAAAEEJINFUfulQokD8gufzepvyxEzLwPl7qtigh/gOO6kcpCRdN8P31yHbl4jqryhooBg=="
            });
            builder.Entity<Company>().HasData(new Company
            {
                Id = 1,
                CompanyName = "BilgeAdam",
                CompanyTitle = "Eğitim Kurumu",
                Logo = imageData,
                PhoneNumber = "5413221526",
                Address = "İstanbul",
                Email = "huseyin@gmail.com",
                Status = true
            });
            builder.Entity<AppRole>().HasData(new AppRole
            {
                Id=Guid.NewGuid(),
                Name="SiteManager",
                NormalizedName="SITEMANAGER"

            });
            builder.Entity<AppRole>().HasData(new AppRole
            {
                Id=Guid.NewGuid(),
                Name="CompanyManager",
                NormalizedName="COMPANYMANAGER"
            });
            builder.Entity<AppRole>().HasData(new AppRole
            {
                Id = Guid.NewGuid(),
                Name = "Employee",
                NormalizedName = "EMPLOYEE"
            });

            



            base.OnModelCreating(builder);
        }
    }
}
