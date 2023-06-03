namespace Bilgeadam.HrPlatform.UI.Areas.SiteManager.Models
{
    public class ErrorViewModelSiteManager
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}