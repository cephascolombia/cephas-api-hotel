using Hotel.Domain.Common;

namespace Hotel.Domain.Entities
{
    public class Reservation : IAuditableEntity
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
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedDate { get; set; }
        public string ReservationCode { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public virtual Room? Room { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual ReservationStatus? ReservationStatus { get; set; }
        public virtual PaymentStatus? PaymentStatus { get; set; }
    }
}
