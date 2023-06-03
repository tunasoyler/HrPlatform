using AutoMapper;
using Bilgeadam.HrPlatform.Entities.DTOs;
using Bilgeadam.HrPlatform.Entities.Entities;
using Bilgeadam.HrPlatform.UI.Areas.SiteManager.Models;
using Bilgeadam.HrPlatform.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Diagnostics;
using Image = SixLabors.ImageSharp.Image;

namespace Bilgeadam.HrPlatform.UI.Areas.SiteManager.Controllers
{
    [Area("SiteManager")]
    [Authorize(Roles = "SiteManager")]
    public class SiteManagerController : Controller
    {
        private readonly ILogger<SiteManagerController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<AppUser> _signInManager;
        public SiteManagerController(ILogger<SiteManagerController> logger, UserManager<AppUser> userManager, IMapper mapper, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
        }


        public async Task<IActionResult> Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                //AppUser user = await _userManager.FindByEmailAsync(email);
                AppUser user = _signInManager.UserManager.GetUserAsync(User).Result;
                return View(user);
            }
            return RedirectToAction("Login", "User");

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModelSiteManager { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Detail(AppUser user)
        {
            user = _signInManager.UserManager.GetUserAsync(User).Result;
            return View(user);
        }
        public async Task<IActionResult> Update(string id)
        {
           AppUser user = _signInManager.UserManager.GetUserAsync(User).Result;
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Update(AppUser userToUpdate, IFormFile photo)
        {
            ModelState.Remove("TcNo");
            ModelState.Remove("photo");
            ModelState.Remove("Gender");
            ModelState.Remove("FirstName");
            ModelState.Remove("LastName");
            ModelState.Remove("BirthPlace");
            AppUser user = await _userManager.FindByIdAsync(userToUpdate.Id.ToString());
            if (ModelState.IsValid)
            {
               
                user = UpdatePhoto(photo, user);
                //user.SecurityStamp = Guid.NewGuid().ToString();
                user.PhoneNumber = userToUpdate.PhoneNumber;
                user.Address = userToUpdate.Address;

                await _userManager.UpdateAsync(user);

                return RedirectToAction("Index");
            }
          
            return View(user);
          
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
    }
}