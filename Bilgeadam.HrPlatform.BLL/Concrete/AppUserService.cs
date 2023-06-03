using Bilgeadam.HrPlatform.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilgeadam.HrPlatform.BLL.Concrete
{
    public class AppUserService : UserManager<AppUser>
    {
        public AppUserService(IUserStore<AppUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<AppUser> passwordHasher, IEnumerable<IUserValidator<AppUser>> userValidators, IEnumerable<IPasswordValidator<AppUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<AppUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {

        }
        public override async Task<IdentityResult> CreateAsync(AppUser user, string password)
        {
            string firstname = ConvertTurkishCharacters(user.FirstName);
            string lastname =ConvertTurkishCharacters(user.LastName);
            user.Email = firstname + "." + lastname + "@bilgeadam.com";
            var result = await base.CreateAsync(user, password);
            return result;
        }
        public string ConvertTurkishCharacters(string input)
        {
            string normalized = input.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
