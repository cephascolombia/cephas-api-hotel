using Hotel.Application.DTOs.Common;
using Hotel.Application.DTOs.RoomsAmenitiesImages;
using Hotel.Application.Interfaces.Repositories;
using Hotel.Application.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hotel.Application.Services
{
    public class RoomAmenitiesImagesService : IRoomAmenitiesImagesService
    {
        private readonly IRoomAmenitiesImagesRepository _repository;
        private readonly IAmenityService _amenityService;

        public RoomAmenitiesImagesService(IRoomAmenitiesImagesRepository repository, IAmenityService amenityService)
        {
            _repository = repository;
            _amenityService = amenityService;
        }

        public async Task<int> CreateAsync(CreateRoomFullDto dto)
        {
            if (dto.Amenities != null && dto.Amenities.Any())
            {
                foreach (var amenityId in dto.Amenities)
                {
                    var amenity = await _amenityService.GetByIdAsync(amenityId);
                    if (amenity == null)
                    {
                        throw new System.Exception($"La amenidad con ID {amenityId} no existe.");
                    }
                }
            }

            return await _repository.CreateAsync(
                dto.Name,
                dto.PricePerNight,
                dto.CreatedBy,
                dto.Capacity,
                dto.Description,
                dto.Amenities?.ToArray(),
                dto.Images?.ToArray()
            );
        }

        public async Task<bool> UpdateAsync(int id, UpdateRoomFullDto dto)
        {
            if (dto.Amenities != null && dto.Amenities.Any())
            {
                foreach (var amenityId in dto.Amenities)
                {
                    var amenity = await _amenityService.GetByIdAsync(amenityId);
                    if (amenity == null)
                    {
                        throw new System.Exception($"La amenidad con ID {amenityId} no existe.");
                    }
                }
            }

            var amenitiesJson = dto.Amenities != null ? JsonSerializer.Serialize(dto.Amenities) : "[]";
            var imagesJson = dto.Images != null ? JsonSerializer.Serialize(dto.Images) : "[]";

            await _repository.UpdateAsync(
                id,
                dto.Name,
                dto.PricePerNight,
                dto.Capacity,
                dto.Description,
                amenitiesJson,
                imagesJson
            );

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task<RoomFullDto?> GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (result == null) return null;

            var dto = new RoomFullDto
            {
                RoomId = result.room_id,
                Name = result.name,
                PricePerNight = result.price_per_night,
                Capacity = result.capacity,
                Description = result.description,
                IsActive = result.is_active,
                CreatedDate = result.created_date,
                CreatedBy = result.created_by,
                Amenities = !string.IsNullOrEmpty(result.amenities) && result.amenities != "null"
                    ? JsonSerializer.Deserialize<List<RoomAmenityDto>>(result.amenities) ?? new()
                    : new(),
                Images = !string.IsNullOrEmpty(result.images) && result.images != "null"
                    ? JsonSerializer.Deserialize<List<RoomImageDto>>(result.images) ?? new()
                    : new()
            };

            return dto;
        }

        public async Task<PagedResponseDto<RoomFullListDto>> GetPagedAsync(RoomFilterDto filter)
        {
            var (data, total) = await _repository.GetPagedAsync(
                filter.Page,
                filter.PageSize,
                filter.Name,
                filter.Capacity,
                filter.MinPrice,
                filter.MaxPrice,
                filter.Sort
            );

            var dtos = data.Select(r => new RoomFullListDto
            {
                RoomId = r.room_id,
                Name = r.name,
                PricePerNight = r.price_per_night,
                Capacity = r.capacity,
                Description = r.description,
                IsAvailable = r.is_available,
                Amenities = !string.IsNullOrEmpty(r.amenities) && r.amenities != "null"
                    ? JsonSerializer.Deserialize<List<RoomAmenityDto>>(r.amenities) ?? new()
                    : new(),
                Images = !string.IsNullOrEmpty(r.images) && r.images != "null"
                    ? JsonSerializer.Deserialize<List<RoomImageListDto>>(r.images) ?? new()
                    : new()
            }).ToList();

            return new PagedResponseDto<RoomFullListDto>
            {
                Data = dtos,
                TotalRecords = total,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }
    }
}
