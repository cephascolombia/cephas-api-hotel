using Hotel.Application.DTOs.Common;
using Hotel.Application.DTOs.RoomsAmenitiesImages;
using System.Threading.Tasks;

namespace Hotel.Application.Interfaces.Services
{
    public interface IRoomAmenitiesImagesService
    {
        Task<int> CreateAsync(CreateRoomFullDto dto);
        Task<bool> UpdateAsync(int id, UpdateRoomFullDto dto);
        Task<bool> DeleteAsync(int id);
        Task<RoomFullDto?> GetByIdAsync(int id);
        Task<PagedResponseDto<RoomFullListDto>> GetPagedAsync(RoomFilterDto filter);
    }
}
