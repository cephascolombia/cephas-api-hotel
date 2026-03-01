using Hotel.Application.DTOs.Amenities;
using Hotel.Application.DTOs.Common;
using Hotel.Application.Interfaces.Commons;
using Hotel.Application.Interfaces.Services;
using Hotel.Domain.Entities;
using System.Linq.Expressions;

namespace Hotel.Application.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly IGenericRepository<Amenity> _repository;

        public AmenityService(IGenericRepository<Amenity> repository)
            => _repository = repository;

        public async Task<int> CreateAsync(CreateAmenityDto dto)
        {
            var amenity = new Amenity
            {
                Name = dto.Name,
                CreatedBy = dto.CreatedBy,
                IsActive = true
            };

            // SaveChangesAsync y el interceptor de fechas
            await _repository.AddAsync(amenity);
            return amenity.AmenityId;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var amenity = await _repository.GetByIdAsync(id);
            if (amenity == null) return false;

            amenity.IsActive = false; // Borrado lógico
            await _repository.UpdateAsync(amenity);
            return true;
        }

        public async Task<AmenityDto?> GetByIdAsync(int id)
        {
            var amenity = await _repository.GetByConditionIgnoringFiltersAsync(a => a.AmenityId == id);
            if (amenity == null) return null;

            return new AmenityDto
            {
                AmenityId = amenity.AmenityId,
                Name = amenity.Name,
                IsActive = amenity.IsActive
            };
        }

        public async Task<PagedResponseDto<AmenityDto>> GetPagedAsync(
            PagedRequestDto request)
        {
            Expression<Func<Amenity, bool>>? filter = null;
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var searchTerm = request.Search.ToLower();
                filter = a => a.Name.ToLower().Contains(searchTerm);
            }

            var (data, total) = await _repository.GetPagedAsync(request.Page, 
                request.PageSize, filter);

            return new PagedResponseDto<AmenityDto>
            {
                Data = data.Select(a => new AmenityDto
                {
                    AmenityId = a.AmenityId,
                    Name = a.Name,
                    IsActive = a.IsActive
                }),
                TotalRecords = total,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task<bool> UpdateAsync(int id, CreateAmenityDto dto)
        {
            var amenity = await _repository.GetByIdAsync(id);
            if (amenity == null) return false;

            amenity.Name = dto.Name;

            await _repository.UpdateAsync(amenity);
            return true;
        }
    }
}
