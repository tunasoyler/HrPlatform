using AutoMapper;
using Bilgeadam.HrPlatform.BLL.Abstract;
using Bilgeadam.HrPlatform.DAL.Abstract;
using Bilgeadam.HrPlatform.Entities.DTOs;
using Bilgeadam.HrPlatform.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.WebPages.Html;

namespace Bilgeadam.HrPlatform.BLL.Concrete
{
    public class CompanyManagerServices : ICompanyManagerServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper; // şimdilik işlevsiz
        private readonly UserManager<AppUser> userManager;

        public CompanyManagerServices(IUnitOfWork _unitOfWork, IMapper _mapper, UserManager<AppUser> _userManager)
        {
            this.unitOfWork=_unitOfWork;
            this.mapper=_mapper;
            this.userManager=_userManager;
        }

        public async Task ChangeCompanyManagerStatus(string id, bool status)
        {
            AppUser appUser = await userManager.FindByIdAsync(id.ToString());
            appUser.Status = status;
            if (status == false)
            {
                appUser.ContractEndDate = DateTime.Now;
            }
            else
            {
                appUser.DateOfRecruitment = DateTime.Now;
            }
            await userManager.UpdateAsync(appUser);
        }

        public async Task CreateCompanyManager(CompanyManager companyManager)
        {
            await unitOfWork.CompanyManager.AddAsync(companyManager);
            await unitOfWork.CommitAsync();
        }
        public async Task DeleteCompany(int id)
        {
            CompanyManager companyManager = await unitOfWork.CompanyManager.GetByIdAsync(id);
            unitOfWork.CompanyManager.Remove(companyManager);
            await unitOfWork.CommitAsync();
        }

        public async Task<List<SelectListItem>> FillCompany()
        {

            List<SelectListItem> classLevelList = unitOfWork.QueryableCompanyList.Select(x => new SelectListItem()
            {
                Text = x.CompanyName,
                Value = x.Id.ToString()
            }).ToList();

            return classLevelList;
        }

        public async Task<IEnumerable<CompanyManager>> GetAllCompanyManager()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AppUser> GetCompanyManager()
        {

            //IList<AppUser> appUsers = await userManager.GetUsersInRoleAsync("CompanyManager");
            IEnumerable<AppUser> appUsers =  unitOfWork.Company.GetCompanyManagerWithCompany();
            return appUsers;


        }

        public async Task<CompanyManager> GetCompanyManagerById(int Id)
        {
            return await unitOfWork.CompanyManager.GetByIdAsync(Id);
        }

        public AppUser GetCompanyManagerWithCompany(string id)
        {
          return unitOfWork.CompanyManager.GetUserWithCompany(id);
        }

        public async Task UpdateCompanyManager(CompanyManager companyManager, int id)
        {
           await unitOfWork.CompanyManager.Update(companyManager, id);
        }
    }
}
