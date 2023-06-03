namespace Bilgeadam.HrPlatform.UI.Areas.CompanyManager.Models
{
    public class ErrorViewModelCompanyManager
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}