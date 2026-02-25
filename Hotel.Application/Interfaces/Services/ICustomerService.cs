using Hotel.Application.DTOs.Common;
using Hotel.Application.DTOs.Customers;

namespace Hotel.Application.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllAsync();
        Task<PagedResponseDto<CustomerDto>> GetPagedAsync(PagedRequestDto request);
        Task<CustomerDto?> GetByIdAsync(int id);
        Task<ProcessResult<int>> CreateAsync(CreateCustomerDto dto);
        Task<ProcessResult<bool>> UpdateAsync(int id, UpdateCustomerDto dto);
        Task<ProcessResult<bool>> DeleteAsync(int id);
    }
}
