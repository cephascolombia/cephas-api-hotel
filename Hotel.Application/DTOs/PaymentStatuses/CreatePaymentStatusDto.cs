namespace Hotel.Application.DTOs.PaymentStatuses
{
    public class CreatePaymentStatusDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsFinal { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}
