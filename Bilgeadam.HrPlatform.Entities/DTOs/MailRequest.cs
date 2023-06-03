using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilgeadam.HrPlatform.Entities.DTOs
{
    public class MailRequest
    {
        public string? Name { get; set; }
        public string? SenderEmail { get; set; }
        public string? ReceiverEmail { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
    }
}
