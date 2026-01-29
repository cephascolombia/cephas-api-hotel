namespace Hotel.Application.DTOs.ReservationStatuses
{
    public class ReservationStatusDto
    {
        public int ReservationStatusId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public bool IsFinal { get; set; }
    }
}
