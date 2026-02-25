using Hotel.Application.DTOs.Common;
using Hotel.Application.DTOs.Rooms;
using Hotel.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/rooms")]
    public class RoomsController : ApiControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
            => _roomService = roomService;

        // GET: api/v1/rooms/paged
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] PagedRequestDto request)
        {
            var result = await _roomService.GetPagedAsync(request);
            return Ok(result);
        }

        // GET: api/v1/rooms/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await _roomService.GetByIdAsync(id);
            if (room == null) return NotFound(new ApiErrorResponse { 
                Status = 404, 
                Message = "Habitación no encontrada.", 
                Error = null });

            return Ok(room);
        }

        // POST: api/v1/rooms
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoomDto dto)
        {
            // El ApiController valida automáticamente los DataAnnotations del DTO (Precio > 0, etc)
            var roomId = await _roomService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = roomId }, dto);
        }

        // PUT: api/v1/rooms/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateRoomDto dto)
        {
            var updated = await _roomService.UpdateAsync(id, dto);
            if (!updated) return NotFound(new ApiErrorResponse { 
                Status = 404, 
                Message = "Habitación no encontrada.", 
                Error = null });

            return NoContent();
        }

        // DELETE: api/v1/rooms/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _roomService.DeleteAsync(id);
            if (!deleted) return NotFound(new ApiErrorResponse { 
                Status = 404, 
                Message = "Habitación no encontrada.", 
                Error = null });

            return NoContent();
        }
    }
}