using Bilgeadam.HrPlatform.Entities.DTOs;
using Bilgeadam.HrPlatform.Entities.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.WebPages.Html;

namespace Bilgeadam.HrPlatform.BLL.Abstract
{
    public interface ICompanyManagerServices
    {
        Task<IEnumerable<CompanyManager>> GetAllCompanyManager();
        Task<CompanyManager> GetCompanyManagerById(int Id);

        Task CreateCompanyManager(CompanyManager company);
        Task UpdateCompanyManager(CompanyManager company, int id);
        Task DeleteCompany(int id);
        Task<List<SelectListItem>> FillCompany();
        IEnumerable<AppUser> GetCompanyManager();
        AppUser GetCompanyManagerWithCompany(string id);
        Task ChangeCompanyManagerStatus(string id, bool status);
    }
}
