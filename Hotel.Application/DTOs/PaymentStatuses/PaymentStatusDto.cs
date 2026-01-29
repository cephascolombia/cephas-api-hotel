namespace Hotel.Application.DTOs.PaymentStatuses
{
    public class PaymentStatusDto
    {
        public int PaymentStatusId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsFinal { get; set; }
    }
}
