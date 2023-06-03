using AutoMapper;
using Bilgeadam.HrPlatform.BLL.Abstract;
using Bilgeadam.HrPlatform.DAL.Abstract;
using Bilgeadam.HrPlatform.Entities.DTOs;
using Bilgeadam.HrPlatform.Entities.Entities;

namespace Bilgeadam.HrPlatform.BLL.Concrete
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CompanyService(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }

        public async Task<Company> CreateCompany(Company company)
        {
            await unitOfWork.Company.AddAsync(company);
            await unitOfWork.CommitAsync();
            return company;
        }

        public async Task DeleteCompany(int id)
        { 
            Company company = await unitOfWork.Company.GetByIdAsync(id);
            unitOfWork.Company.Remove(company);
            await unitOfWork.CommitAsync();
        }
        

        public async Task<IEnumerable<CompanyDto>> GetAllCompany()
        {
            var Companies = await unitOfWork.Company.GetAllAsync();
            var CompanyDtos = mapper.Map<IEnumerable<Company>, IEnumerable<CompanyDto>>(Companies);
            return CompanyDtos;
        }

        public async Task<Company> GetCompanyById(int Id)
        {
            var company = await unitOfWork.Company.GetByIdAsync(Id);
            return company;
        }

        public async Task ChangeCompanyStatus(int Id,bool status)
        {
            Company company = await unitOfWork.Company.GetByIdAsync(Id);
            company.Status = status;
            if (status == false)
            {
                company.ContactEndDate = DateTime.Now;
            }
            else
            {
                company.ContactStartDate = DateTime.Now;
            }
            await UpdateCompany(company, company.Id);
            
        }

        public async Task UpdateCompany(Company company, int id)
        {
            //_context.Entry(unchanged).CurrentValues.SetValues(entity);
            await unitOfWork.Company.Update(company, id);
            await unitOfWork.CommitAsync();
        }
    }
}
