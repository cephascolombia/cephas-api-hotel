using Hotel.Application.DTOs.Common;
using Hotel.Application.DTOs.RoomsAmenitiesImages;
using Hotel.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class RoomsAmenitiesImagesController : ControllerBase
    {
        private readonly IRoomAmenitiesImagesService _service;

        public RoomsAmenitiesImagesController(IRoomAmenitiesImagesService service)
            => _service = service;


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoomFullDto dto)
        {
            try
            {
                var id = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id }, new { id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoomFullDto dto)
        {
            try
            {
                var success = await _service.UpdateAsync(id, dto);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _service.DeleteAsync(id);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponseDto<RoomFullListDto>>> GetPaged([FromQuery] RoomFilterDto filter)
        {
            var result = await _service.GetPagedAsync(filter);
            return Ok(result);
        }
    }
}
