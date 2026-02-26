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

        public async Task<ProcessResult<int>> CreateAsync(CreateCustomerDto dto)
        {
            // 1. Validación de Email
            var (existingEmail, totalEmail) = await _repository.GetPagedAsync(
                1, 1, c => c.Email == dto.Email);

            if (totalEmail > 0)
            {
                return ProcessResult<int>.Failure(
                    $"El correo electrónico '{dto.Email}' ya está registrado.");
            }

            // 2. Validación de IdentityDocument (si se provee)
            if (!string.IsNullOrWhiteSpace(dto.IdentityDocument))
            {
                var (existingDoc, totalDoc) = await _repository.GetPagedAsync(
                    1, 1, c => c.IdentityDocument == dto.IdentityDocument);

                if (totalDoc > 0)
                {
                    return ProcessResult<int>.Failure(
                        $"El documento de identidad '{dto.IdentityDocument}' ya está registrado.");
                }
            }

            // 3. Validación de Phone (si se provee)
            if (!string.IsNullOrWhiteSpace(dto.Phone))
            {
                var (existingPhone, totalPhone) = await _repository.GetPagedAsync(
                    1, 1, c => c.Phone == dto.Phone);

                if (totalPhone > 0)
                {
                    return ProcessResult<int>.Failure(
                        $"El número de teléfono '{dto.Phone}' ya está registrado.");
                }
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
            return ProcessResult<int>.Success(customer.CustomerId);
        }

        public async Task<ProcessResult<bool>> UpdateAsync(int id, UpdateCustomerDto dto)
        {
            var customer = await _repository.GetByIdAsync(id);

            if (customer == null)
                return ProcessResult<bool>.Failure("Cliente no encontrado.");

            // 1. Validación de Email duplicado (excluyendo al cliente actual)
            var emailLower = dto.Email.ToLower().Trim();
            var (_, totalEmail) = await _repository.GetPagedAsync(
                1, 1, c => c.Email.ToLower() == emailLower && c.CustomerId != id);

            if (totalEmail > 0)
            {
                return ProcessResult<bool>.Failure(
                    $"El correo electrónico '{dto.Email}' ya está registrado por otro cliente.");
            }

            // 2. Validación de IdentityDocument duplicado (si se provee)
            if (!string.IsNullOrWhiteSpace(dto.IdentityDocument))
            {
                var docLower = dto.IdentityDocument.ToLower().Trim();
                var (_, totalDoc) = await _repository.GetPagedAsync(
                    1, 1, c => c.IdentityDocument != null && c.IdentityDocument.ToLower() == docLower && c.CustomerId != id);

                if (totalDoc > 0)
                {
                    return ProcessResult<bool>.Failure(
                        $"El documento de identidad '{dto.IdentityDocument}' ya está registrado por otro cliente.");
                }
            }

            // 3. Validación de Phone duplicado (si se provee)
            if (!string.IsNullOrWhiteSpace(dto.Phone))
            {
                var phoneLower = dto.Phone.ToLower().Trim();
                var (_, totalPhone) = await _repository.GetPagedAsync(
                    1, 1, c => c.Phone != null && c.Phone.ToLower() == phoneLower && c.CustomerId != id);

                if (totalPhone > 0)
                {
                    return ProcessResult<bool>.Failure(
                        $"El número de teléfono '{dto.Phone}' ya está registrado por otro cliente.");
                }
            }

            customer.FullName = dto.FullName;
            customer.Email = dto.Email;
            customer.Phone = dto.Phone;
            customer.IdentityDocument = dto.IdentityDocument;
            customer.Address = dto.Address;
            customer.Notes = dto.Notes;
            
            if (dto.IsActive.HasValue)
            {
                customer.IsActive = dto.IsActive.Value;
            }

            await _repository.UpdateAsync(customer);
            return ProcessResult<bool>.Success(true);
        }

        public async Task<ProcessResult<bool>> DeleteAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);

            if (customer == null)
                return ProcessResult<bool>.Failure("Cliente no encontrado.");
            
            customer.IsActive = false;

            await _repository.UpdateAsync(customer);
            return ProcessResult<bool>.Success(true);
        }
    }
}
