using Hotel.Application.DTOs.Common;
using Hotel.Application.DTOs.Rooms;

namespace Hotel.Application.Interfaces.Services
{
    public interface IRoomService
    {
        Task<PagedResponseDto<RoomDto>> GetPagedAsync(PagedRequestDto request);
        Task<RoomDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateRoomDto dto);
        Task<bool> UpdateAsync(int id, CreateRoomDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
