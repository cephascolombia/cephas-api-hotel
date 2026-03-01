using Hotel.Application.DTOs.Common;
using Hotel.Application.DTOs.Reservations;
using Hotel.Application.Interfaces.Commons;
using Hotel.Application.Interfaces.Services;
using Hotel.Domain.Entities;
using System.Linq.Expressions;

namespace Hotel.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IGenericRepository<Reservation> _repository;

        public ReservationService(IGenericRepository<Reservation> repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateAsync(CreateReservationDto dto)
        {
            var reservation = new Reservation
            {
                RoomId = dto.RoomId,
                CustomerId = dto.CustomerId,
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                ReservationStatusId = dto.ReservationStatusId,
                PaymentStatusId = dto.PaymentStatusId,
                TotalAmount = dto.TotalAmount,
                PaidAmount = dto.PaidAmount,
                ReservationCode = dto.ReservationCode,
                CreatedBy = dto.CreatedBy,
                IsActive = true
            };

            await _repository.AddAsync(reservation);
            return reservation.ReservationId;
        }

        public async Task<PagedResponseDto<ReservationDto>> GetPagedAsync(PagedRequestDto request)
        {
            Expression<Func<Reservation, bool>>? filter = null;
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var searchTerm = request.Search.ToLower();
                filter = r => r.ReservationCode.ToLower().Contains(searchTerm);
            }

            var (data, total) = await _repository.GetPagedAsync(request.Page, request.PageSize, filter);

            return new PagedResponseDto<ReservationDto>
            {
                Data = data.Select(r => new ReservationDto
                {
                    ReservationId = r.ReservationId,
                    RoomId = r.RoomId,
                    CustomerId = r.CustomerId,
                    CheckInDate = r.CheckInDate,
                    CheckOutDate = r.CheckOutDate,
                    ReservationStatusId = r.ReservationStatusId,
                    PaymentStatusId = r.PaymentStatusId,
                    TotalAmount = r.TotalAmount,
                    PaidAmount = r.PaidAmount,
                    ReservationCode = r.ReservationCode,
                    IsActive = r.IsActive
                }),
                TotalRecords = total,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task<ReservationDto?> GetByIdAsync(int id)
        {
            var reservation = await _repository.GetByIdAsync(id);
            if (reservation == null) return null;

            return new ReservationDto
            {
                ReservationId = reservation.ReservationId,
                RoomId = reservation.RoomId,
                CustomerId = reservation.CustomerId,
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                ReservationStatusId = reservation.ReservationStatusId,
                PaymentStatusId = reservation.PaymentStatusId,
                TotalAmount = reservation.TotalAmount,
                PaidAmount = reservation.PaidAmount,
                ReservationCode = reservation.ReservationCode,
                IsActive = reservation.IsActive
            };
        }

        public async Task<bool> UpdateAsync(int id, CreateReservationDto dto)
        {
            var reservation = await _repository.GetByIdAsync(id);
            if (reservation == null) return false;

            reservation.RoomId = dto.RoomId;
            reservation.CustomerId = dto.CustomerId;
            reservation.CheckInDate = dto.CheckInDate;
            reservation.CheckOutDate = dto.CheckOutDate;
            reservation.ReservationStatusId = dto.ReservationStatusId;
            reservation.PaymentStatusId = dto.PaymentStatusId;
            reservation.TotalAmount = dto.TotalAmount;
            reservation.PaidAmount = dto.PaidAmount;
            reservation.ReservationCode = dto.ReservationCode;
            reservation.UpdatedDate = DateTime.UtcNow;

            await _repository.UpdateAsync(reservation);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reservation = await _repository.GetByIdAsync(id);
            if (reservation == null) return false;

            reservation.IsActive = false; // Borrado lógico
            await _repository.UpdateAsync(reservation);
            return true;
        }
    }
}
