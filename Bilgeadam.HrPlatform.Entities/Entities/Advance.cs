using Bilgeadam.HrPlatform.Entities.Enums;

namespace Bilgeadam.HrPlatform.Entities.Entities
{
    public class Advance : BaseEntity
    {
        public Guid EmployeeId { get; set; }
        public AppUser Employee { get; set; }
        public DateTime ReceievedTime { get; set; } = DateTime.Now;
        public CurrencyType CurrencyType { get; set; }
        public int Amount { get; set; }
        public AdvanceStatus AdvanceStatus { get; set; } = AdvanceStatus.OnStandBy;
        public AdvanceType AdvanceType { get; set; } 
        public string? Description { get; set; }
    }
}