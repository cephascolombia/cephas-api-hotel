using Hotel.Application.DTOs.Common;
using Hotel.Application.DTOs.Reservations;
using Hotel.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/reservations")]
    public class ReservationsController : ApiControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        // GET: api/v1/reservations/paged
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] PagedRequestDto request)
        {
            var result = await _reservationService.GetPagedAsync(request);
            return Ok(result);
        }

        // GET: api/v1/reservations/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);
            if (reservation == null) return NotFound(new ApiErrorResponse { Status = 404, Message = "Reserva no encontrada.", Error = null });

            return Ok(reservation);
        }

        // POST: api/v1/reservations
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservationDto dto)
        {
            var reservationId = await _reservationService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = reservationId }, dto);
        }

        // PUT: api/v1/reservations/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateReservationDto dto)
        {
            var updated = await _reservationService.UpdateAsync(id, dto);
            if (!updated) return NotFound(new ApiErrorResponse { Status = 404, Message = "Reserva no encontrada.", Error = null });

            return NoContent();
        }

        // DELETE: api/v1/reservations/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _reservationService.DeleteAsync(id);
            if (!deleted) return NotFound(new ApiErrorResponse { Status = 404, Message = "Reserva no encontrada.", Error = null });

            return NoContent();
        }
    }
}
