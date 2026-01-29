using System.ComponentModel.DataAnnotations;

namespace Hotel.Application.DTOs.ReservationStatuses
{
    public class CreateReservationStatusDto
    {
        [Required(ErrorMessage = "El código de estado es obligatorio.")]
        [MaxLength(20)]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del estado es obligatorio.")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Range(1, 100, ErrorMessage = "El orden debe estar entre 1 y 100.")]
        public int SortOrder { get; set; }

        public bool IsFinal { get; set; }

        [Required]
        public string CreatedBy { get; set; } = string.Empty;
    }
}
