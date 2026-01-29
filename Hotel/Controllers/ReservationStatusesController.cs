using Hotel.Application.DTOs.Common;
using Hotel.Application.DTOs.ReservationStatuses;
using Hotel.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/reservation-statuses")]
    [ApiController]
    public class ReservationStatusesController : ControllerBase
    {
        private readonly IReservationStatusService _statusService;

        public ReservationStatusesController(IReservationStatusService statusService)
        {
            _statusService = statusService;
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResponseDto<ReservationStatusDto>>> GetPaged([FromQuery] PagedRequestDto request)
        {
            var result = await _statusService.GetPagedAsync(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationStatusDto>> GetById(int id)
        {
            var result = await _statusService.GetByIdAsync(id);
            if (result == null) return NotFound(new { Message = $"Estado con ID {id} no encontrado." });

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateReservationStatusDto dto)
        {
            var id = await _statusService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateReservationStatusDto dto)
        {
            var updated = await _statusService.UpdateAsync(id, dto);
            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _statusService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}