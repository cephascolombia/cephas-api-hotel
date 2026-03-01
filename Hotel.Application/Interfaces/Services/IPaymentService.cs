using Hotel.Application.DTOs.Common;
using Hotel.Application.DTOs.Payments;

namespace Hotel.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<int> CreateAsync(CreatePaymentDto dto);
        Task<PagedResponseDto<PaymentDto>> GetPagedAsync(PagedRequestDto request);
        Task<PaymentDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, CreatePaymentDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
