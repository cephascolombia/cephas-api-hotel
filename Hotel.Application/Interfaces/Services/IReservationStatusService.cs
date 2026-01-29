using Hotel.Application.DTOs.Common;
using Hotel.Application.DTOs.ReservationStatuses;

namespace Hotel.Application.Interfaces.Services
{
    public interface IReservationStatusService
    {
        Task<PagedResponseDto<ReservationStatusDto>> GetPagedAsync(PagedRequestDto request);
        Task<ReservationStatusDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateReservationStatusDto dto);
        Task<bool> UpdateAsync(int id, CreateReservationStatusDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
