using Bilgeadam.HrPlatform.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilgeadam.HrPlatform.BLL.MailConfigurationService
{
    public interface IMailService
    {
        public string SendPasswordToMail(AppUser user);
    }
}
