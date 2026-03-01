namespace Hotel.Application.DTOs.Reservations
{
    public class ReservationDto
    {
        public int ReservationId { get; set; }
        public int RoomId { get; set; }
        public int CustomerId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int ReservationStatusId { get; set; }
        public int PaymentStatusId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public string ReservationCode { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
