using AutoMapper;
using Bilgeadam.HrPlatform.BLL.Abstract;
using Bilgeadam.HrPlatform.BLL.MailConfigurationService;
using Bilgeadam.HrPlatform.Entities.DTOs;
using Bilgeadam.HrPlatform.Entities.Entities;
using Bilgeadam.HrPlatform.UI.Areas.SiteManager.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Reflection;
using System.Security.Policy;
using Microsoft.AspNetCore.Authorization;

namespace Bilgeadam.HrPlatform.UI.Areas.CompanyManager.Controllers
{
    [Area("SiteManager")]
    [Authorize(Roles = "SiteManager")]

    public class CompanyManagerController : Controller
    {
        private readonly ILogger<SiteManagerController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ICompanyService _companyService;
        private readonly ICompanyManagerServices _companyManagerService;
        private readonly IMailService mailService;
        public CompanyManagerController(ILogger<SiteManagerController> logger, UserManager<AppUser> userManager, IMapper mapper, ICompanyService companyService, ICompanyManagerServices companyManagerService, IMailService mailService)
        {
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
            _companyService = companyService;
            _companyManagerService = companyManagerService;
            this.mailService = mailService;
        }

        public async Task<IActionResult> UpdateCompanyManager(string id)
        {

            AppUser appUser = await _userManager.FindByIdAsync(id);
            CompanyManagerUpdateDto companyManagerUpdateDto = _mapper.Map<AppUser, CompanyManagerUpdateDto>(appUser);
            return View(companyManagerUpdateDto);
            //return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCompanyManager(CompanyManagerUpdateDto companyManagerUpdateDto, IFormFile photo)
        {
            ModelState.Remove("photo");
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                AppUser appUser = await _userManager.FindByIdAsync(companyManagerUpdateDto.Id);
                appUser = CreatePhoto(appUser, photo);
                appUser.ContractEndDate = companyManagerUpdateDto.ContractEndDate;
                appUser.PhoneNumber = companyManagerUpdateDto.PhoneNumber;
                appUser.Address = companyManagerUpdateDto.Address;


                if (photo == null)
                {
                    AppUser profilePhoto = await _userManager.FindByIdAsync(companyManagerUpdateDto.Id);
                    appUser.ProfilePhoto = profilePhoto.ProfilePhoto;
                }

                await _userManager.UpdateAsync(appUser);
                return RedirectToAction("GetCompanyManager");
            }
            else
            {
                AppUser profilePhoto = await _userManager.FindByIdAsync(companyManagerUpdateDto.Id);
                companyManagerUpdateDto.ProfilePhoto = profilePhoto.ProfilePhoto;
                return View(companyManagerUpdateDto);
            }

        }
        public async Task<IActionResult> DeleteCompanyManager(string id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            if (appUser.Status == true)
            {
                await _companyManagerService.ChangeCompanyManagerStatus(id, false);
            }
            else
            {
                await _companyManagerService.ChangeCompanyManagerStatus(id, true);
            }
            return RedirectToAction("GetCompanyManager");
        }
        public async Task<IActionResult> GetCompanyManager()

        {
            IEnumerable<AppUser> appUserss = _companyManagerService.GetCompanyManager();
            return View(appUserss);
        }
        public async Task<IActionResult> CreateCompanyManager()
        {
            CompanyManagerDto companyManagerDto = new CompanyManagerDto();
            companyManagerDto.CompanyForDropDown = await _companyManagerService.FillCompany();
            return View(companyManagerDto);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCompanyManager(CompanyManagerDto companyManagerdto, IFormFile photo)
        {

            int companyid;
            if (companyManagerdto.CompanyId.HasValue)
            {
                companyid = companyManagerdto.CompanyId.Value;
                companyManagerdto.Company = await _companyService.GetCompanyById(companyid);
            }
            ModelState.Remove("photo");
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                AppUser appUser = _mapper.Map<CompanyManagerDto, AppUser>(companyManagerdto);
                appUser = CreatePhoto(appUser, photo);
                IdentityResult result = await _userManager.CreateAsync(appUser);
                var password = mailService.SendPasswordToMail(appUser);
                await _userManager.AddPasswordAsync(appUser, password);
                if (result.Succeeded)
                {
                    //await _userManager.AddToRoleAsync(appUser, "MEMBER");
                    await _userManager.UpdateAsync(appUser);
                    await _userManager.AddToRoleAsync(appUser, "CompanyManager");
                    return RedirectToAction("Index"); //todo:Şu an GetcompanyManager mevcut değil 
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("UserCreateErr", error.Description);
                    }
                }
                return RedirectToAction("GetCompanyManager");
            }

            companyManagerdto.CompanyForDropDown = await _companyManagerService.FillCompany();
            return View(companyManagerdto);
        }
        public async Task<IActionResult> DetailCompanyManager(string id)
        {

            AppUser appUser = _companyManagerService.GetCompanyManagerWithCompany(id);

            return View(appUser);
        }
        public AppUser CreatePhoto(AppUser appuser, IFormFile photo)
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
                        appuser.ProfilePhoto = ms.ToArray();
                    }
                }
            }
            return appuser;
        }
    }
}