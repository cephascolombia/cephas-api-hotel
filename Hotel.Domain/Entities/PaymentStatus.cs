using Hotel.Domain.Common;

namespace Hotel.Domain.Entities
{
    public class PaymentStatus : IAuditableEntity
    {
        public int PaymentStatusId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsFinal { get; set; }
        // Auditoría
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
