using AutoMapper;
using Bilgeadam.HrPlatform.BLL.Abstract;
using Bilgeadam.HrPlatform.BLL.Concrete;
using Bilgeadam.HrPlatform.BLL.MailConfigurationService;
using Bilgeadam.HrPlatform.DAL.Concrete;
using Bilgeadam.HrPlatform.Entities.DTOs;
using Bilgeadam.HrPlatform.Entities.Entities;
using Bilgeadam.HrPlatform.UI.Areas.SiteManager.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Data;

namespace Bilgeadam.HrPlatform.UI.Areas.CompanyManager.Controllers
{
    [Area("CompanyManager")]
    [Authorize(Roles = "SiteManager,CompanyManager")]
    public class CMController : Controller
    {
        
        private readonly ILogger<CMController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ICompanyService _companyService;
        private readonly ICompanyManagerServices _companyManagerService;
        private readonly IMailService _mailService;
        public CMController(ILogger<CMController> logger, UserManager<AppUser> userManager, IMapper mapper, SignInManager<AppUser> signInManager, ICompanyService companyManager, ICompanyManagerServices companyManagerService, IMailService mailService)
        {
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _companyService = companyManager;
            _companyManagerService = companyManagerService;
            _mailService = mailService;
        }

        Company company = new Company();

        public async Task<IActionResult> Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                //AppUser user = await _userManager.FindByEmailAsync(email);
                AppUser user = _signInManager.UserManager.GetUserAsync(User).Result;
                int companyid;
                if (user.CompanyId.HasValue)
                {
                    companyid = user.CompanyId.Value;
                    company = await _companyService.GetCompanyById(companyid);
                }
                
                return View(user);
            }
            return RedirectToAction("Login", "CM");
        }
        public async Task<IActionResult> CreateEmployee()
        {
            CompanyManagerDto employee = new CompanyManagerDto();
            employee.CompanyForDropDown = await _companyManagerService.FillCompany();
            AppUser companyManager = _signInManager.UserManager.GetUserAsync(User).Result;
            ViewBag.CompanyID = companyManager.CompanyId;
            return View(employee);
        }
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(CompanyManagerDto employeeDto)
        {
            AppUser employee = _mapper.Map<CompanyManagerDto, AppUser>(employeeDto);
            employee.UserName = employeeDto.FirstName + employee.LastName;
            IdentityResult result = await _userManager.CreateAsync(employee);
            var password = _mailService.SendPasswordToMail(employee);
            await _userManager.AddPasswordAsync(employee, password);

            if (result.Succeeded)

            {
                //await _userManager.AddToRoleAsync(appUser, "MEMBER");
                await _userManager.UpdateAsync(employee);
                await _userManager.AddToRoleAsync(employee, "Employee");
                return RedirectToAction("EmployeeList", "CM");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("UserCreateErr", error.Description);
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> EmployeeList()
        {
            IList<AppUser> employees = await _userManager.GetUsersInRoleAsync("Employee");
            List<AppUser> employeesWithCompany=new List<AppUser>();
            foreach (AppUser employee in employees)
            {
                int companyid;
                if (employee.CompanyId.HasValue)
                {
                    companyid = employee.CompanyId.Value;
                    employee.Company = await _companyService.GetCompanyById(companyid);
                }
                employeesWithCompany.Add(employee);
            }
            return View(employees);
        }

        public async Task<IActionResult> Update()
        {
            AppUser user = _signInManager.UserManager.GetUserAsync(User).Result;
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Update(AppUser userToUpdate, IFormFile photo)
        {
            AppUser user = await _userManager.FindByIdAsync(userToUpdate.Id.ToString());
            user = UpdatePhoto(photo, user);
            //user.SecurityStamp = Guid.NewGuid().ToString();
            user.PhoneNumber = userToUpdate.PhoneNumber;
            user.Address = userToUpdate.Address;

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Index");
        }

        public AppUser UpdatePhoto(IFormFile photo, AppUser user) //Bu metodu dış servise'a taşı
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
                        user.ProfilePhoto = ms.ToArray();
                    }
                }
            }
            return user;
        }
        public async Task<IActionResult> UpdateEmployee(string id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            CompanyManagerUpdateDto companyManagerUpdateDto = _mapper.Map<AppUser, CompanyManagerUpdateDto>(appUser);
            return View(companyManagerUpdateDto);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(CompanyManagerUpdateDto companyManagerUpdateDto, IFormFile photo)
        {
            ModelState.Remove("photo");
            ModelState.Remove("Gender");
            if (ModelState.IsValid)
            {
                AppUser appUser = await _userManager.FindByIdAsync(companyManagerUpdateDto.Id);
                appUser = UpdatePhoto(photo, appUser);
                appUser.ContractEndDate = companyManagerUpdateDto.ContractEndDate;
                appUser.PhoneNumber = companyManagerUpdateDto.PhoneNumber;
                appUser.Address = companyManagerUpdateDto.Address;
                appUser.Wage = Convert.ToDouble(companyManagerUpdateDto.Wage);

                if (photo == null)
                {
                    AppUser profilePhoto = await _userManager.FindByIdAsync(companyManagerUpdateDto.Id);
                    appUser.ProfilePhoto = profilePhoto.ProfilePhoto;
                }

                await _userManager.UpdateAsync(appUser);
                return RedirectToAction("EmployeeList");
            }
            else
            {
                AppUser profilePhoto = await _userManager.FindByIdAsync(companyManagerUpdateDto.Id);
                companyManagerUpdateDto.ProfilePhoto = profilePhoto.ProfilePhoto;
                return View(companyManagerUpdateDto);
            }

        }
        public async Task<IActionResult> DetailCompanyManager(AppUser user)
        {

            user = _signInManager.UserManager.GetUserAsync(User).Result;

            return View(user);
        }
        public async Task<IActionResult> EmployeeDetail(string id)
        {

            AppUser appUser = _companyManagerService.GetCompanyManagerWithCompany(id);

            return View(appUser);
        }
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            if (appUser.Status==true) 
            {
               await _companyManagerService.ChangeCompanyManagerStatus(id, false);
            }
            else
            {
               await _companyManagerService.ChangeCompanyManagerStatus(id, true);
            }
            return RedirectToAction("EmployeeList");
        }

    }
}
