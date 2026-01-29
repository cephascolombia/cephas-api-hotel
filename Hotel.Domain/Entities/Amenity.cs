using Hotel.Domain.Common;

namespace Hotel.Domain.Entities
{
    public class Amenity : IAuditableEntity
    {
        public int AmenityId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
