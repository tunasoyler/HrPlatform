using AutoMapper;
using Bilgeadam.HrPlatform.BLL.MailConfigurationService;
using Bilgeadam.HrPlatform.Entities.DTOs;
using Bilgeadam.HrPlatform.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Bilgeadam.HrPlatform.UI.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        //private readonly MailService _mailService;
        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            //_mailService = mailService;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserDTO signIn, IFormFile photo)
        {
            ModelState.Remove("photo");
            if (ModelState.IsValid)
            {
                //AppUser appUser = new()
                //{
                //    UserName = signIn.UserName,
                //    Address = signIn.Address,
                //    FirstName = signIn.FirstName,
                //    LastName = signIn.LastName,
                //    SecondName = signIn.SecondName,
                //    SecondLastName = signIn.SecondLastName,
                //    BirthDate = signIn.BirthDate,
                //    Department = signIn.Department,
                //    BirthPlace = signIn.BirthPlace,
                //    Profession = signIn.Profession,
                //    TCNo = signIn.TCNo,

                //    //Address = signIn.Address,
                //    PhoneNumber = signIn.PhoneNumber,
                //};
                AppUser appUser = _mapper.Map<UserDTO, AppUser>(signIn);
                //appUser.Email = CreateEmail(appUser.FirstName, appUser.LastName);
                appUser = CreatePhoto(appUser, photo);
                MailService _mailService = new MailService(_userManager);
                //var password = _mailService.SendPasswordToMail(appUser);

                IdentityResult result = await _userManager.CreateAsync(appUser, signIn.Password);
             await   _userManager.AddToRoleAsync(appUser,"SiteManager");

                if (result.Succeeded)

                {
                    //await _userManager.AddToRoleAsync(appUser, "MEMBER");
                    await _userManager.UpdateAsync(appUser);
                    return RedirectToAction("Login", "User"); //todo:Şu an index mevcut değil 
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("UserCreateErr", error.Description);
                    }
                }
            }
            return View("Register", signIn);
        }
        public async Task<IActionResult> Login(string returnUrl)
        {
            await _signInManager.SignOutAsync();
            return View(new Login() { ReturnUrl = returnUrl});
        }
        [HttpPost] 
        public async Task<IActionResult> Login(Login login)
        {
            
            if (ModelState.IsValid)
            {
                string email = login.Email;
                AppUser user = await _userManager.FindByEmailAsync(email);
                
                
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    var message = string.Empty;

                    Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, true);

                    if (signInResult.Succeeded)
                    {
                        #region Roller Oluşturulduğunda Kullanıcının Rolüne Göre Giriş Yapma
                        var roles = await _userManager.GetRolesAsync(user);

                        if (roles.Contains("SiteManager"))
                        {
                            return RedirectToAction("Index", "SiteManager", new { area = "SiteManager" });
                        }
                        else if (roles.Contains("CompanyManager"))
                        {
                            return RedirectToAction("Index", "CM", new { area = "CompanyManager" });
                        }
                        else if (roles.Contains("Employee"))
                        {
                            return RedirectToAction("Index", "Employee", new { area = "Employee" });
                        }
                        //if (!string.IsNullOrWhiteSpace(login.ReturnUrl))
                        //{
                        //    return Redirect(login.ReturnUrl);
                        //}

                        //if (roles.Contains("Member"))
                        //{
                        //    return RedirectToAction("HomePage", "Customer");
                        //}
                        #endregion
                    }
                    else if (signInResult.IsLockedOut)
                    {
                        var lockOutEnd = await _userManager.GetLockoutEndDateAsync(user);
                        ModelState.AddModelError("", $"Your account has suspended for {(lockOutEnd.Value.UtcDateTime - DateTime.UtcNow).Minutes} minutes");
                    }
                    else
                    {


                        if (user != null)
                        {
                            var failedCount = await _userManager.GetAccessFailedCountAsync(user);
                            message = $"I f you failed {(_userManager.Options.Lockout.MaxFailedAccessAttempts - failedCount)} more times.Your account will be suspended ";
                        }
                        else
                        {
                            message = "Invalid password or E-mail address";
                        }
                    }

                    ModelState.AddModelError("", message);
                }
            }
            
            return View(login);
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
        private string CreateEmail(string firstName, string lastName)
        {
            firstName = ConvertToEnglish(firstName);
            lastName = ConvertToEnglish(lastName);
            string email = firstName+"."+lastName+"@bilgeadam.com";
            return email;
        }
        private string ConvertToEnglish(string word)
        {
            word = word.ToLower().Trim();
            word = word.Replace('ö', 'o');
            word = word.Replace('ü', 'u');
            word = word.Replace('ğ', 'g');
            word = word.Replace('ş', 's');
            word = word.Replace('ı', 'i');
            word = word.Replace('ç', 'c');
            return word;
        }
    }
}
