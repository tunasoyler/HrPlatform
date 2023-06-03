using AutoMapper;
using Bilgeadam.HrPlatform.BLL.Abstract;
using Bilgeadam.HrPlatform.DAL.Abstract;
using Bilgeadam.HrPlatform.Entities.DTOs;
using Bilgeadam.HrPlatform.Entities.Entities;
using Bilgeadam.HrPlatform.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilgeadam.HrPlatform.BLL.Concrete
{
    public class AdvanceService : IAdvanceService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AdvanceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Advance> GetAdvanceById(int Id)
        {
            var advance = await unitOfWork.Advance.GetByIdAsync(Id);
            return advance;
        }

        public async Task<IEnumerable<AdvanceDTO>> GetAllAdvances()
        {
            var advances = await unitOfWork.Advance.GetAllAsync();
            var advanceDto = mapper.Map<IEnumerable<Advance>, IEnumerable<AdvanceDTO>>(advances);
            return advanceDto;
        }

        public async Task<Advance> CreateAdvance(Advance advance)
        {
            await unitOfWork.Advance.AddAsync(advance);
            await unitOfWork.CommitAsync();
            return advance;
        }

        public async Task DeleteAdvance(int id)
        {
            Advance advance = await unitOfWork.Advance.GetByIdAsync(id);
            unitOfWork.Advance.Remove(advance);
            await unitOfWork.CommitAsync();
        }

        public async Task UpdateAdvance(Advance advance, int id)
        {
            await unitOfWork.Advance.Update(advance, id);
            await unitOfWork.CommitAsync();
        }
        public async Task ChangeAdvanceStatus(int Id, AdvanceStatus advanceStatus)
        {
            Advance advance = await unitOfWork.Advance.GetByIdAsync(Id);
            advance.AdvanceStatus = advanceStatus;
            await UpdateAdvance(advance, advance.Id);
        }

        public async Task ChangeAdvanceType(int Id, AdvanceType advanceType)
        {
            Advance advance = await unitOfWork.Advance.GetByIdAsync(Id);
            advance.AdvanceType = advanceType;
            await UpdateAdvance(advance, advance.Id);
        }

        public async Task ChangeCurrencyType(int Id, CurrencyType currencyType)
        {
            Advance advance = await unitOfWork.Advance.GetByIdAsync(Id);
            advance.CurrencyType = currencyType;
            await UpdateAdvance(advance, advance.Id);
        }

        public IEnumerable<AdvanceDTO> GetAdvanceByEmployeeId(string id)
        {
            var advances = unitOfWork.Advance.GetAdvancesWithEmployee(id);
            var advanceDto = mapper.Map<IEnumerable<Advance>, IEnumerable<AdvanceDTO>>(advances);
            return advanceDto;
        }
        public IEnumerable<Advance> GetAdvanceByCompanyId(string id)
        {
            var advances = unitOfWork.Advance.GetAdvancesWithCompany(id);
            //var advanceDto = mapper.Map<IEnumerable<Advance>, IEnumerable<AdvanceDTO>>(advances);
            return advances;
        }
    }
}
