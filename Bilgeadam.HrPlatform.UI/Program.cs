using Bilgeadam.HrPlatform.BLL.DependencyResolver.Microsoft;
using Bilgeadam.HrPlatform.DAL.Context;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Microsoft.EntityFrameworkCore;

namespace Bilgeadam.HrPlatform.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddDependcy();
            var app = builder.Build();
           

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseMigrationsEndPoint();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapAreaControllerRoute(
                areaName: "CompanyManager",
                name: "CompanyManager",
                pattern: "CompanyManagerArea/{controller=CM}/{action=Index}/{id?}");

            app.MapAreaControllerRoute(
                areaName: "SiteManager",
                name: "SiteManager",
                pattern: "SiteManagerArea/{controller=SiteManager}/{action=Index}/{id?}");

            app.MapAreaControllerRoute(
                areaName: "Employee",
                name: "Employee",
                pattern: "EmployeeArea/{controller=Employee}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=User}/{action=Login}/{id?}");

            app.Run();
        }
    }
}