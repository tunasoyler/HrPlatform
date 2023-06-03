using Bilgeadam.HrPlatform.Entities.DTOs;
using Bilgeadam.HrPlatform.Entities.Entities;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilgeadam.HrPlatform.BLL.MailConfigurationService
{
    public class MailService : IMailService
    {
        MimeMessage mimeMessage = new MimeMessage();
        SmtpClient smtpClient = new();
        UserManager<AppUser> userManager;

        public MailService(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public string SendPasswordToMail(AppUser user)
        {
            MailboxAddress mailboxAddressFrom = new MailboxAddress("SiteManager", "wisemen.hrplatform@gmail.com");

            mimeMessage.From.Add(mailboxAddressFrom);

            MailboxAddress mailboxAddressTo = new MailboxAddress("User", user.Email);

            mimeMessage.To.Add(mailboxAddressTo);

            mimeMessage.Subject = "Your password in this email. Please don't share with anyone!";

            Guid guid = Guid.NewGuid();
            string password = guid.ToString();
            password = password.Substring(password.Length - 10, 10).ToLower();

            userManager.AddPasswordAsync(user, password).Wait();

            var bodyBuilder = new BodyBuilder()
            {
                TextBody = "Hello! Welcome to the Wisemen-HrPlatform. Here is your password to login the platform:" +
                "\n" +
                "\n" + password
            };

            mimeMessage.Body = bodyBuilder.ToMessageBody();

            smtpClient.Connect("smtp.gmail.com", 587, false);
            smtpClient.Authenticate("wisemen.hrplatform@gmail.com", "mugxyicedkpotpsx");

            smtpClient.Send(mimeMessage);
            smtpClient.Disconnect(true);

            return password;
        }
    }
}
