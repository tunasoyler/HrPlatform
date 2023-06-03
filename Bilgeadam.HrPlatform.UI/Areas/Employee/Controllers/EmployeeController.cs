using AutoMapper;
using Bilgeadam.HrPlatform.BLL.Abstract;
using Bilgeadam.HrPlatform.BLL.MailConfigurationService;
using Bilgeadam.HrPlatform.Entities.DTOs;
using Bilgeadam.HrPlatform.Entities.Entities;
using Bilgeadam.HrPlatform.Entities.Enums;
using Bilgeadam.HrPlatform.UI.Areas.CompanyManager.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Bilgeadam.HrPlatform.UI.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "SiteManager,Employee")]

    public class EmployeeController : Controller
    {
        private readonly ILogger<CMController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ICompanyService _companyService;
        private readonly ICompanyManagerServices _companyManagerService;
        private readonly IMailService _mailService;
        private readonly IAdvanceService _advanceService;

        public EmployeeController(IMailService mailService, ILogger<CMController> logger, UserManager<AppUser> userManager, IMapper mapper, SignInManager<AppUser> signInManager, ICompanyService companyService, ICompanyManagerServices companyManagerService, IAdvanceService advanceService)
        {
            _mailService = mailService;
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _companyService = companyService;
            _companyManagerService = companyManagerService;
            _advanceService = advanceService;
        }
        Company company = new Company();
        public async Task<IActionResult> Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
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
        public async Task<IActionResult> Detail(AppUser user)
        {
            user = _signInManager.UserManager.GetUserAsync(User).Result;
            return View(user);
        }
        public async Task<IActionResult> Update(AppUser user)
        {
            user = _signInManager.UserManager.GetUserAsync(User).Result;
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

        public async Task<IActionResult> RequestAdvance()
        {
            Advance advance = new Advance()
            {
                AdvanceType = AdvanceType.Corporate
            };

            return View(advance);
        }

        [HttpPost]
        public async Task<IActionResult> RequestAdvance(Advance advance)
        {
            ModelState.Remove("Employee");
            AppUser user = _signInManager.UserManager.GetUserAsync(User).Result;
            double wage;
            if (ModelState.IsValid)
            {
                await _advanceService.CreateAdvance(advance);
                return RedirectToAction("MyAdvances");
            }
            return View(advance);
        }
        public async Task<IActionResult> ListAdvance(AppUser employee)
        {
            employee = _signInManager.UserManager.GetUserAsync(User).Result;
            string employeeId = employee.Id.ToString();
            var advances = _advanceService.GetAdvanceByEmployeeId(employeeId);
            return View(advances);
        }
        public ActionResult DeleteAdvance(Advance advance)
        {
            _advanceService.DeleteAdvance(advance.Id);
            return RedirectToAction("ListAdvance");
        }
        public async Task<ActionResult> AdvanceDetail(Advance advance)
        {
            Advance advancetosee = await _advanceService.GetAdvanceById(advance.Id);
            return View(advancetosee);
        }

        public AppUser UpdatePhoto(IFormFile photo, AppUser user)
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
                        var encoder = new JpegEncoder { Quality = 80 };
                        image.Save(ms, encoder);
                        user.ProfilePhoto = ms.ToArray();
                    }
                }
            }
            return user;
        }
    }

}
