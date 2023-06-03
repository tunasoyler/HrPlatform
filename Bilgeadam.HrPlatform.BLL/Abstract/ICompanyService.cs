using Bilgeadam.HrPlatform.Entities.DTOs;
using Bilgeadam.HrPlatform.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilgeadam.HrPlatform.BLL.Abstract
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDto>> GetAllCompany();
        Task<Company> GetCompanyById(int Id);
        Task ChangeCompanyStatus(int Id, bool status);
        Task<Company> CreateCompany(Company company);
        Task UpdateCompany(Company company, int id);
        Task DeleteCompany(int id);
    }
}
