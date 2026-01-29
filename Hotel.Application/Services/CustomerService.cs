using Hotel.Application.DTOs.Common;
using Hotel.Application.DTOs.Customers;
using Hotel.Application.Interfaces.Commons;
using Hotel.Application.Interfaces.Services;
using Hotel.Domain.Entities;
using Hotel.Domain.Exceptions;
using System.Linq.Expressions;

namespace Hotel.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IGenericRepository<Customer> _repository;

        public CustomerService(IGenericRepository<Customer> repository)
            => _repository = repository;

        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            var customers = await _repository.GetAllAsync();

            return customers.Select(c => new CustomerDto
            {
                CustomerId = c.CustomerId,
                FullName = c.FullName,
                Email = c.Email,
                Phone = c.Phone,
                IdentityDocument = c.IdentityDocument,
                Address = c.Address,
                Notes = c.Notes,
                CreatedDate = c.CreatedDate,
                CreatedBy = c.CreatedBy,
                IsActive = c.IsActive
            });
        }


        public async Task<PagedResponseDto<CustomerDto>> GetPagedAsync(
            PagedRequestDto request)
        {
            Expression<Func<Customer, bool>>? filter = null;

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var searchTerm = request.Search.ToLower(); 

                filter = c =>
                    c.FullName.ToLower().Contains(searchTerm) || 
                    c.Email.ToLower().Contains(searchTerm);      
            }

            var result = await _repository.GetPagedAsync(
                request.Page,
                request.PageSize,
                filter
            );

            return new PagedResponseDto<CustomerDto>
            {
                Data = result.Data.Select(MapToDto),
                TotalRecords = result.TotalRecords,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }

        private static CustomerDto MapToDto(Customer customer)
        {
            return new CustomerDto
            {
                CustomerId = customer.CustomerId,
                FullName = customer.FullName,
                Email = customer.Email,
                Phone = customer.Phone,
                IdentityDocument = customer.IdentityDocument,
                Address = customer.Address,
                Notes = customer.Notes,
                CreatedDate = customer.CreatedDate,
                CreatedBy = customer.CreatedBy,
                IsActive = customer.IsActive
            };
        }

        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null)
                return null;

            return new CustomerDto
            {
                CustomerId = customer.CustomerId,
                FullName = customer.FullName,
                Email = customer.Email,
                Phone = customer.Phone,
                IdentityDocument = customer.IdentityDocument,
                Address = customer.Address,
                Notes = customer.Notes,
                CreatedDate = customer.CreatedDate,
                CreatedBy = customer.CreatedBy,
                IsActive = customer.IsActive
            };
        }

        public async Task<int> CreateAsync(CreateCustomerDto dto)
        {
            // Validación de lógica de negocio, ¿ya existe un email igual?
            var (existing, total) = await _repository.GetPagedAsync(
                1, 1, c => c.Email == dto.Email);

            if (total > 0)
            {
                throw new BusinessException(
                    $"El correo electrónico '{dto.Email}' ya está registrado.");
            }

            var customer = new Customer
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                IdentityDocument = dto.IdentityDocument,
                Address = dto.Address,
                Notes = dto.Notes,
                CreatedBy = dto.CreatedBy,
                IsActive = true
            };

            await _repository.AddAsync(customer);
            return customer.CustomerId;
        }

        public async Task<bool> UpdateAsync(int id, UpdateCustomerDto dto)
        {
            var customer = await _repository.GetByIdAsync(id);

            if (customer == null)
                return false;

            customer.FullName = dto.FullName;
            customer.Email = dto.Email;
            customer.Phone = dto.Phone;
            customer.IdentityDocument = dto.IdentityDocument;
            customer.Address = dto.Address;
            customer.Notes = dto.Notes;
            customer.IsActive = dto.IsActive;

            await _repository.UpdateAsync(customer);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);

            if (customer == null)
                return false;
            customer.IsActive = false;

            await _repository.UpdateAsync(customer);
            return true;
        }
    }
}
