using Hotel.Application.DTOs.Amenities;
using Hotel.Application.DTOs.Common;

namespace Hotel.Application.Interfaces.Services
{
    public interface IAmenityService
    {
        Task<PagedResponseDto<AmenityDto>> GetPagedAsync(PagedRequestDto request);
        Task<AmenityDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateAmenityDto dto);
        Task<bool> UpdateAsync(int id, CreateAmenityDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
