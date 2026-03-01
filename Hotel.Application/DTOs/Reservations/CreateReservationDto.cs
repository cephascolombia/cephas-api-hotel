using System.ComponentModel.DataAnnotations;

namespace Hotel.Application.DTOs.Reservations
{
    public class CreateReservationDto
    {
        [Required]
        public int RoomId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Required]
        public int ReservationStatusId { get; set; }

        [Required]
        public int PaymentStatusId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal PaidAmount { get; set; }

        [Required]
        [MaxLength(6)]
        public string ReservationCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string CreatedBy { get; set; } = string.Empty;
    }
}
