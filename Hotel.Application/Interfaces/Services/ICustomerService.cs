using Hotel.Application.DTOs.Common;
using Hotel.Application.DTOs.Customers;

namespace Hotel.Application.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllAsync();
        Task<CustomerDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateCustomerDto dto);
        Task<bool> UpdateAsync(int id, UpdateCustomerDto dto);
        Task<bool> DeleteAsync(int id);
        Task<PagedResponseDto<CustomerDto>> GetPagedAsync(PagedRequestDto request);
    }
}
