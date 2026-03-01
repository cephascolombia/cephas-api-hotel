namespace Hotel.Application.DTOs.Amenities
{
    public class AmenityDto
    {
        public int AmenityId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
