using Hotel.Application.DTOs.Common;
using Hotel.Application.DTOs.PaymentStatuses;

namespace Hotel.Application.Interfaces.Services
{
    public interface IPaymentStatusService
    {
        Task<PagedResponseDto<PaymentStatusDto>> GetPagedAsync(PagedRequestDto request);
        Task<PaymentStatusDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreatePaymentStatusDto dto);
        Task<bool> UpdateAsync(int id, CreatePaymentStatusDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
