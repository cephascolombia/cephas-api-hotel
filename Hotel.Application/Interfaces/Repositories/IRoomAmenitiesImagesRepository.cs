using Hotel.Application.DTOs.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hotel.Application.Interfaces.Repositories
{
    public interface IRoomAmenitiesImagesRepository
    {
        Task<int> CreateAsync(
            string name, 
            decimal pricePerNight, 
            string createdBy, 
            int capacity, 
            string description, 
            bool isWorking,
            int[]? amenities, 
            string[]? images);
            
        Task UpdateAsync(
            int roomId, 
            string name, 
            decimal pricePerNight, 
            int capacity, 
            string description, 
            bool isWorking,
            string amenitiesJson, 
            string imagesJson);
            
        Task DeleteAsync(int roomId);
        
        Task<RoomFullQueryResult?> GetByIdAsync(int roomId);
        
        Task<(IEnumerable<RoomFullListQueryResult> Data, int TotalRecords)> GetPagedAsync(
            int page, 
            int pageSize, 
            string? name, 
            int? capacity, 
            decimal? minPrice, 
            decimal? maxPrice, 
            string sort);
    }

    // Auxiliar classes for mapping DB results
    public class RoomFullQueryResult
    {
        public int room_id { get; set; }
        public string name { get; set; } = string.Empty;
        public decimal price_per_night { get; set; }
        public int capacity { get; set; }
        public string description { get; set; } = string.Empty;
        public bool is_active { get; set; }
        public bool is_working { get; set; }
        public System.DateTime created_date { get; set; }
        public string created_by { get; set; } = string.Empty;
        public string? amenities { get; set; }
        public string? images { get; set; }
    }

    public class RoomFullListQueryResult
    {
        public int room_id { get; set; }
        public string name { get; set; } = string.Empty;
        public decimal price_per_night { get; set; }
        public int capacity { get; set; }
        public string description { get; set; } = string.Empty;
        public bool is_available { get; set; }
        public bool is_working { get; set; }
        public string? amenities { get; set; }
        public string? images { get; set; }
        public int total_records { get; set; }
    }
}
