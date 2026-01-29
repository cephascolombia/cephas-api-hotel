namespace Hotel.Application.DTOs.Customers
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? IdentityDocument { get; set; }
        public string? Address { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
