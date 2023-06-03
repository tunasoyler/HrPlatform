namespace Bilgeadam.HrPlatform.Entities.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Today;
        public DateTime UpdatedDate { get; set; } = DateTime.Today;
    }
}