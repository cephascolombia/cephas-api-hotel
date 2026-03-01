using System.ComponentModel.DataAnnotations;

namespace Hotel.Application.DTOs.Payments
{
    public class CreatePaymentDto
    {
        [Required]
        public int ReservationId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Reference { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string CreatedBy { get; set; } = string.Empty;
    }
}
