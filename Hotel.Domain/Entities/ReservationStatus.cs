using Hotel.Domain.Common;

namespace Hotel.Domain.Entities
{
    public class ReservationStatus : IAuditableEntity
    {
        public int ReservationStatusId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public bool IsFinal { get; set; }
        // Propiedades de auditoría
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
