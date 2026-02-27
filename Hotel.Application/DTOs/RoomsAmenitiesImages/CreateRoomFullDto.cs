using System.ComponentModel.DataAnnotations;

namespace Hotel.Application.DTOs.RoomsAmenitiesImages
{
    public class CreateRoomFullDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal PricePerNight { get; set; }

        [Range(1, 20, ErrorMessage = "La capacidad debe ser al menos de 1 persona")]
        public int Capacity { get; set; } = 1;

        public string Description { get; set; } = string.Empty;

        public string CreatedBy { get; set; } = "Admin";

        public List<int>? Amenities { get; set; }
        
        public List<string>? Images { get; set; }
    }
}
