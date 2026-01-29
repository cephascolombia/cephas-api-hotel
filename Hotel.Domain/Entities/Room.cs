using Hotel.Domain.Common;

namespace Hotel.Domain.Entities
{
    public class Room : IAuditableEntity
    {
        public int RoomId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}
