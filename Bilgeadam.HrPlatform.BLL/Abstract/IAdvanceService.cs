using Bilgeadam.HrPlatform.Entities.DTOs;
using Bilgeadam.HrPlatform.Entities.Entities;
using Bilgeadam.HrPlatform.Entities.Enums;

namespace Bilgeadam.HrPlatform.BLL.Abstract
{
    public interface IAdvanceService
    {
        Task<IEnumerable<AdvanceDTO>> GetAllAdvances();
        Task<Advance> GetAdvanceById(int Id);
        IEnumerable<AdvanceDTO> GetAdvanceByEmployeeId(string Id);
        IEnumerable<Advance> GetAdvanceByCompanyId(string id);
        Task ChangeAdvanceStatus(int Id, AdvanceStatus advanceStatus);
        Task ChangeAdvanceType(int Id, AdvanceType advanceType);
        Task ChangeCurrencyType(int Id, CurrencyType currencyType);
        Task<Advance> CreateAdvance(Advance company);
        Task UpdateAdvance(Advance company, int id);
        Task DeleteAdvance(int id);
    }
}
