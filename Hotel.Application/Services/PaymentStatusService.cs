using Hotel.Application.DTOs.Common;
using Hotel.Application.DTOs.PaymentStatuses;
using Hotel.Application.Interfaces.Commons;
using Hotel.Application.Interfaces.Services;
using Hotel.Domain.Entities;
using System.Linq.Expressions;

namespace Hotel.Application.Services
{
    public class PaymentStatusService : IPaymentStatusService
    {
        private readonly IGenericRepository<PaymentStatus> _repository;

        public PaymentStatusService(IGenericRepository<PaymentStatus> repository)
            => _repository = repository;

        public async Task<int> CreateAsync(CreatePaymentStatusDto dto)
        {
            var status = new PaymentStatus
            {
                Code = dto.Code.ToUpper().Trim(),
                Name = dto.Name,
                IsFinal = dto.IsFinal,
                CreatedBy = dto.CreatedBy
            };

            await _repository.AddAsync(status);
            return status.PaymentStatusId;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var status = await _repository.GetByIdAsync(id);
            if (status == null) return false;

            status.IsActive = false;
            await _repository.UpdateAsync(status);
            return true;
        }

        public async Task<PaymentStatusDto?> GetByIdAsync(int id)
        {
            var status = await _repository.GetByIdAsync(id);
            return status == null ? null : new PaymentStatusDto
            {
                PaymentStatusId = status.PaymentStatusId,
                Code = status.Code,
                Name = status.Name,
                IsFinal = status.IsFinal
            };
        }

        public async Task<PagedResponseDto<PaymentStatusDto>> GetPagedAsync(PagedRequestDto request)
        {
            Expression<Func<PaymentStatus, bool>>? filter = null;

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var searchTerm = request.Search.ToLower();
                filter = x => x.Name.ToLower().Contains(searchTerm) ||
                             x.Code.ToLower().Contains(searchTerm);
            }

            var (data, total) = await _repository.GetPagedAsync(
                request.Page,
                request.PageSize,
                filter,
                q => q.OrderBy(x => x.Name) // Ordenamos alfabéticamente por defecto
            );

            return new PagedResponseDto<PaymentStatusDto>
            {
                Data = data.Select(x => new PaymentStatusDto
                {
                    PaymentStatusId = x.PaymentStatusId,
                    Code = x.Code,
                    Name = x.Name,
                    IsFinal = x.IsFinal
                }),
                TotalRecords = total,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task<bool> UpdateAsync(int id, CreatePaymentStatusDto dto)
        {
            var status = await _repository.GetByIdAsync(id);
            if (status == null) return false;

            status.Code = dto.Code.ToUpper().Trim();
            status.Name = dto.Name;
            status.IsFinal = dto.IsFinal;

            await _repository.UpdateAsync(status);
            return true;
        }
    }
}
