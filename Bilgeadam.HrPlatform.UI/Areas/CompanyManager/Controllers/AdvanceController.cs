using AutoMapper;
using Bilgeadam.HrPlatform.BLL.Abstract;
using Bilgeadam.HrPlatform.BLL.Concrete;
using Bilgeadam.HrPlatform.BLL.MailConfigurationService;
using Bilgeadam.HrPlatform.DAL.Concrete;
using Bilgeadam.HrPlatform.Entities.DTOs;
using Bilgeadam.HrPlatform.Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Bilgeadam.HrPlatform.UI.Areas.CompanyManager.Controllers
{
    [Area("CompanyManager")]
    [Authorize(Roles = "SiteManager,CompanyManager")]

    public class AdvanceController : Controller
    {
        private readonly ILogger<CMController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ICompanyService _companyService;
        private readonly ICompanyManagerServices _companyManagerService;
        private readonly IMailService _mailService;
        private readonly IAdvanceService advanceService;

        public AdvanceController(ILogger<CMController> logger, UserManager<AppUser> userManager, IMapper mapper, SignInManager<AppUser> signInManager, ICompanyService companyService, ICompanyManagerServices companyManagerService, IMailService mailService, IAdvanceService advanceService)
        {
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _companyService = companyService;
            _companyManagerService = companyManagerService;
            _mailService = mailService;
            this.advanceService = advanceService;
        }

        public async Task<IActionResult> Index(AppUser cm)
        {
            cm = _signInManager.UserManager.GetUserAsync(User).Result;
            var companyId = cm.CompanyId.ToString();
            var advances = advanceService.GetAdvanceByCompanyId(companyId);
            return View(advances);
        }
        public async Task<ActionResult> AdvanceDetail(Advance advance)
        {
            Advance advancetosee = await advanceService.GetAdvanceById(advance.Id);
            return View(advancetosee);
        }
        public async Task<ActionResult> ChangeStatus(Advance advance)
        {
            advanceService.ChangeAdvanceStatus(advance.Id, advance.AdvanceStatus);
            Advance advancetoChange = await advanceService.GetAdvanceById(advance.Id);
            return View(advancetoChange);
        }
    }
}
