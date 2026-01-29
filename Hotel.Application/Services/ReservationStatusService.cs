using Hotel.Application.DTOs.Common;
using Hotel.Application.DTOs.ReservationStatuses;
using Hotel.Application.Interfaces.Commons;
using Hotel.Application.Interfaces.Services;
using Hotel.Domain.Entities;
using System.Linq.Expressions;

namespace Hotel.Application.Services
{
    public class ReservationStatusService : IReservationStatusService
    {
        private readonly IGenericRepository<ReservationStatus> _repository;

        public ReservationStatusService(IGenericRepository<ReservationStatus> repository)
            => _repository = repository;

        public async Task<int> CreateAsync(CreateReservationStatusDto dto)
        {
            var status = new ReservationStatus
            {
                Code = dto.Code.ToUpper().Trim(), 
                Name = dto.Name,
                SortOrder = dto.SortOrder,
                IsFinal = dto.IsFinal,
                CreatedBy = dto.CreatedBy,
                IsActive = true
            };

            await _repository.AddAsync(status);
            return status.ReservationStatusId;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var status = await _repository.GetByIdAsync(id);
            if (status == null) return false;

            status.IsActive = false;
            await _repository.UpdateAsync(status);
            return true;
        }

        public async Task<ReservationStatusDto?> GetByIdAsync(int id)
        {
            var status = await _repository.GetByIdAsync(id);
            if (status == null) return null;

            return new ReservationStatusDto
            {
                ReservationStatusId = status.ReservationStatusId,
                Code = status.Code,
                Name = status.Name,
                SortOrder = status.SortOrder,
                IsFinal = status.IsFinal
            };
        }

        public async Task<PagedResponseDto<ReservationStatusDto>> GetPagedAsync(PagedRequestDto request)
        {
            Expression<Func<ReservationStatus, bool>>? filter = null;
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var searchTerm = request.Search.ToLower();
                filter = x => x.Name.ToLower().Contains(searchTerm) || x.Code.ToLower().Contains(searchTerm);
            }

            // Pasamos la función de ordenamiento al repositorio
            var (data, total) = await _repository.GetPagedAsync(
                request.Page,
                request.PageSize,
                filter,
                q => q.OrderBy(x => x.SortOrder) 
            );

            return new PagedResponseDto<ReservationStatusDto>
            {
                Data = data.Select(x => new ReservationStatusDto
                {
                    ReservationStatusId = x.ReservationStatusId,
                    Code = x.Code,
                    Name = x.Name,
                    SortOrder = x.SortOrder,
                    IsFinal = x.IsFinal
                }),
                TotalRecords = total,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task<bool> UpdateAsync(int id, CreateReservationStatusDto dto)
        {
            var status = await _repository.GetByIdAsync(id);
            if (status == null) return false;

            status.Code = dto.Code.ToUpper().Trim();
            status.Name = dto.Name;
            status.SortOrder = dto.SortOrder;
            status.IsFinal = dto.IsFinal;

            await _repository.UpdateAsync(status);
            return true;
        }
    }
}
