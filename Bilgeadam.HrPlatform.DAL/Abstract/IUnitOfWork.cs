using Bilgeadam.HrPlatform.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilgeadam.HrPlatform.DAL.Abstract
{
    public interface IUnitOfWork:IDisposable
    {
        Task<int> CommitAsync();
        ICompanyRepository Company { get; }
        IAdvanceRepository Advance { get; }
        ICompanyManagerRepository CompanyManager { get; }
        IQueryable<Company> QueryableCompanyList { get; }

    }
}
