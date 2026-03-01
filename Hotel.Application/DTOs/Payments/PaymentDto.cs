namespace Hotel.Application.DTOs.Payments
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public int ReservationId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
