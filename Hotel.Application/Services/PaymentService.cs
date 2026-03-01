using Hotel.Application.DTOs.Common;
using Hotel.Application.DTOs.Payments;
using Hotel.Application.Interfaces.Commons;
using Hotel.Application.Interfaces.Services;
using Hotel.Domain.Entities;
using System.Linq.Expressions;

namespace Hotel.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IGenericRepository<Payment> _repository;

        public PaymentService(IGenericRepository<Payment> repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateAsync(CreatePaymentDto dto)
        {
            var payment = new Payment
            {
                ReservationId = dto.ReservationId,
                Amount = dto.Amount,
                PaymentDate = dto.PaymentDate,
                PaymentMethod = dto.PaymentMethod,
                Reference = dto.Reference,
                CreatedBy = dto.CreatedBy,
                IsActive = true
            };

            await _repository.AddAsync(payment);
            return payment.PaymentId;
        }

        public async Task<PagedResponseDto<PaymentDto>> GetPagedAsync(PagedRequestDto request)
        {
            Expression<Func<Payment, bool>>? filter = null;
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var searchTerm = request.Search.ToLower();
                filter = p => p.Reference.ToLower().Contains(searchTerm);
            }

            var (data, total) = await _repository.GetPagedAsync(
                request.Page,
                request.PageSize,
                filter,
                q => q.OrderBy(p => p.PaymentId));

            return new PagedResponseDto<PaymentDto>
            {
                Data = data.Select(p => new PaymentDto
                {
                    PaymentId = p.PaymentId,
                    ReservationId = p.ReservationId,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    PaymentMethod = p.PaymentMethod,
                    Reference = p.Reference,
                    IsActive = p.IsActive
                }),
                TotalRecords = total,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task<PaymentDto?> GetByIdAsync(int id)
        {
            var payment = await _repository.GetByIdAsync(id);
            if (payment == null) return null;

            return new PaymentDto
            {
                PaymentId = payment.PaymentId,
                ReservationId = payment.ReservationId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                PaymentMethod = payment.PaymentMethod,
                Reference = payment.Reference,
                IsActive = payment.IsActive
            };
        }

        public async Task<bool> UpdateAsync(int id, CreatePaymentDto dto)
        {
            var payment = await _repository.GetByIdAsync(id);
            if (payment == null) return false;

            payment.ReservationId = dto.ReservationId;
            payment.Amount = dto.Amount;
            payment.PaymentDate = dto.PaymentDate;
            payment.PaymentMethod = dto.PaymentMethod;
            payment.Reference = dto.Reference;

            await _repository.UpdateAsync(payment);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var payment = await _repository.GetByIdAsync(id);
            if (payment == null) return false;

            payment.IsActive = false; // Borrado lógico
            await _repository.UpdateAsync(payment);
            return true;
        }
    }
}
