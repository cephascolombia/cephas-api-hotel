using Hotel.Application.DTOs.Common;

namespace Hotel.Application.DTOs.RoomsAmenitiesImages
{
    public class RoomFilterDto : PagedRequestDto
    {
        public string? Name { get; set; }
        public int? Capacity { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string Sort { get; set; } = "price";
    }
}
