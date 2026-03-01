using Hotel.Application.DTOs.Common;
using Hotel.Application.DTOs.Rooms;
using Hotel.Application.Interfaces.Commons;
using Hotel.Application.Interfaces.Services;
using Hotel.Domain.Entities;
using System.Linq.Expressions;

namespace Hotel.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IGenericRepository<Room> _repository;

        public RoomService(IGenericRepository<Room> repository)
            => _repository = repository;

        public async Task<int> CreateAsync(CreateRoomDto dto)
        {
            var room = new Room
            {
                Name = dto.Name,
                PricePerNight = dto.PricePerNight,
                Capacity = dto.Capacity,
                Description = dto.Description,
                CreatedBy = dto.CreatedBy,
                IsWorking = dto.IsWorking,
                IsActive = true
            };

            // SaveChangesAsync y el interceptor de fechas
            await _repository.AddAsync(room);
            return room.RoomId;
        }

        public async Task<PagedResponseDto<RoomDto>> GetPagedAsync(PagedRequestDto request)
        {
            Expression<Func<Room, bool>>? filter = null;
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var searchTerm = request.Search.ToLower();
                filter = r => r.Name.ToLower().Contains(searchTerm);
            }

            var (data, total) = await _repository.GetPagedAsync(request.Page, request.PageSize, filter);

            return new PagedResponseDto<RoomDto>
            {
                Data = data.Select(r => new RoomDto
                {
                    RoomId = r.RoomId,
                    Name = r.Name,
                    PricePerNight = r.PricePerNight,
                    Capacity = r.Capacity,
                    Description = r.Description,
                    IsActive = r.IsActive,
                    IsWorking = r.IsWorking
                }),
                TotalRecords = total,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task<RoomDto?> GetByIdAsync(int id)
        {
            var room = await _repository.GetByIdAsync(id);
            if (room == null) return null;

            return new RoomDto
            {
                RoomId = room.RoomId,
                Name = room.Name,
                PricePerNight = room.PricePerNight,
                Capacity = room.Capacity,
                Description = room.Description,
                IsActive = room.IsActive,
                IsWorking = room.IsWorking
            };
        }

        public async Task<bool> UpdateAsync(int id, CreateRoomDto dto)
        {
            var room = await _repository.GetByIdAsync(id);
            if (room == null) return false;

            room.Name = dto.Name;
            room.PricePerNight = dto.PricePerNight;
            room.Capacity = dto.Capacity;
            room.Description = dto.Description;
            room.IsWorking = dto.IsWorking;

            await _repository.UpdateAsync(room);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var room = await _repository.GetByIdAsync(id);
            if (room == null) return false;

            room.IsActive = false; // Borrado lógico
            await _repository.UpdateAsync(room);
            return true;
        }
    }
}
