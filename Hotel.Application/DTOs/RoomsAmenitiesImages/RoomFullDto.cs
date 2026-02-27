using System.Text.Json.Serialization;

namespace Hotel.Application.DTOs.RoomsAmenitiesImages
{
    public class RoomFullDto
    {
        public int RoomId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public List<RoomAmenityDto> Amenities { get; set; } = new();
        public List<RoomImageDto> Images { get; set; } = new();
    }

    public class RoomFullListDto
    {
        public int RoomId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public List<RoomAmenityDto> Amenities { get; set; } = new();
        public List<RoomImageListDto> Images { get; set; } = new();
    }

    public class RoomAmenityDto
    {
        [JsonPropertyName("amenity_id")]
        public int AmenityId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class RoomImageDto
    {
        [JsonPropertyName("image_id")]
        public int ImageId { get; set; }
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; } = string.Empty;
    }

    public class RoomImageListDto
    {
        [JsonPropertyName("image_id")]
        public int ImageId { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }
}
