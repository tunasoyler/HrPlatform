namespace Bilgeadam.HrPlatform.UI.Models
{
    public class DefaultErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}