using Bilgeadam.HrPlatform.Entities.Entities;

namespace Bilgeadam.HrPlatform.DAL.Abstract
{
    public interface ICompanyRepository:IRepository<Company>
    {

        IEnumerable<AppUser> GetCompanyManagerWithCompany();
    }
}
