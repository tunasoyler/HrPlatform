using AutoMapper;
using Bilgeadam.HrPlatform.BLL.Abstract;
using Bilgeadam.HrPlatform.Entities.DTOs;
using Bilgeadam.HrPlatform.Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Data;

namespace Bilgeadam.HrPlatform.UI.Areas.SiteManager.Controllers
{
    [Area("SiteManager")]
    [Authorize(Roles = "SiteManager")]
    public class CompanyController : Controller
    {
        private readonly ILogger<SiteManagerController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ICompanyService _companyService;
        public CompanyController(ILogger<SiteManagerController> logger, UserManager<AppUser> userManager, IMapper mapper, ICompanyService companyService)
        {
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
            _companyService = companyService;

        }
        public async Task<IActionResult> Index()
        {
            var companyDtos = await _companyService.GetAllCompany();
            return View(companyDtos);
        }
        public async Task<IActionResult> UpdateCompany(int id)
        {

            Company company = await _companyService.GetCompanyById(id);
            CompanyUpdateDtos companyUpdateDtos = _mapper.Map<Company, CompanyUpdateDtos>(company);
            return View(companyUpdateDtos);
            //return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCompany(CompanyUpdateDtos companyUpdateDtos, IFormFile photo)
        {
            ModelState.Remove("photo");
            if (ModelState.IsValid)
            {
                Company companyToUpdate = await _companyService.GetCompanyById(companyUpdateDtos.Id);
                companyToUpdate.CompanyName = companyUpdateDtos.CompanyName;
                companyToUpdate.CompanyTitle = companyUpdateDtos.CompanyTitle;
                companyToUpdate.PhoneNumber = companyUpdateDtos.PhoneNumber;
                companyToUpdate.Address = companyUpdateDtos.Address;
                companyToUpdate.Email = companyUpdateDtos.Email;

                companyToUpdate = CreatePhoto(companyToUpdate, photo);
                if (photo == null)
                {
                    Company logo = await _companyService.GetCompanyById(companyUpdateDtos.Id);
                    companyToUpdate.Logo = logo.Logo;
                }

                await _companyService.UpdateCompany(companyToUpdate, companyUpdateDtos.Id);
                return RedirectToAction("Index");
            }
            else
            {
                Company logo = await _companyService.GetCompanyById(companyUpdateDtos.Id);
                companyUpdateDtos.Logo = logo.Logo;
                return View(companyUpdateDtos);
            }

        }
        public async Task<IActionResult> DeleteCompany(CompanyDto companyDto)
        {
            if (companyDto.Status == true)
            {
                await _companyService.ChangeCompanyStatus(companyDto.Id, false);
            }
            else
            {
                await _companyService.ChangeCompanyStatus(companyDto.Id, true);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> CreateCompany()
        {
            Company company = new Company();
            return View(company);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCompany(Company company, IFormFile photo)
        {
            ModelState.Remove("photo");
            if (ModelState.IsValid && company.IsValidContractDates())
            {
                company = CreatePhoto(company, photo);
                await _companyService.CreateCompany(company);
                return RedirectToAction("Index");
            }
            if (!company.IsValidContractDates())
            {
                ModelState.AddModelError(string.Empty, "Contract start date must be before contract end date and contract end date must be before datetime now.");
            }
          

            return View(company);
        }
        public Company CreatePhoto(Company company, IFormFile photo)
        {
            if (photo != null && photo.Length > 0)
            {
                using (var image = Image.Load(photo.OpenReadStream()))
                {
                    var ratio = (float)500 / image.Width;
                    var height = (int)(image.Height * ratio);
                    image.Mutate(x => x.Resize(500, height));
                    using (var ms = new MemoryStream())
                    {
                        var encoder = new JpegEncoder { Quality = 80 }; // %80 sıkıştırma kalitesi
                        image.Save(ms, encoder);
                        company.Logo = ms.ToArray();
                    }
                }
            }
            return company;
        }
        public async Task<IActionResult> DetailCompany(CompanyDto companyDto)
        {
            //Company company = _mapper.Map<CompanyDto, Company>(companyDto);
            Company company = await _companyService.GetCompanyById(companyDto.Id);
            return View(company);
        }
    }
}
