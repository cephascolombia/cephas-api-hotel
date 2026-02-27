using System.ComponentModel.DataAnnotations;

namespace Hotel.Application.DTOs.Amenities
{
    public class CreateAmenityDto
    {
        [Required(ErrorMessage = "El nombre de la amenidad es obligatorio.")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string CreatedBy { get; set; } = string.Empty;
    }
}
