using Hotel.Application.DTOs.Common;
using Hotel.Application.DTOs.Reservations;

namespace Hotel.Application.Interfaces.Services
{
    public interface IReservationService
    {
        Task<int> CreateAsync(CreateReservationDto dto);
        Task<PagedResponseDto<ReservationDto>> GetPagedAsync(PagedRequestDto request);
        Task<ReservationDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, CreateReservationDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
