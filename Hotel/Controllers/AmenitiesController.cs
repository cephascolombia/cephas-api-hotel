using Hotel.Application.DTOs.Amenities;
using Hotel.Application.DTOs.Common;
using Hotel.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/amenities")]
    public class AmenitiesController : ApiControllerBase
    {
        private readonly IAmenityService _amenityService;

        public AmenitiesController(IAmenityService amenityService)
            => _amenityService = amenityService;

        // GET: api/v1/amenities/paged
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] PagedRequestDto request)
        {
            var result = await _amenityService.GetPagedAsync(request);
            return Ok(result);
        }

        // GET: api/v1/amenities/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var amenity = await _amenityService.GetByIdAsync(id);
            if (amenity == null) return NotFound(new ApiErrorResponse { 
                Status = 404, 
                Message = "Amenidad no encontrada.", 
                Error = null });

            return Ok(amenity);
        }

        // POST: api/v1/amenities
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAmenityDto dto)
        {
            // El ApiController valida automáticamente los DataAnnotations del DTO (Precio > 0, etc)
            var amenityId = await _amenityService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = amenityId }, dto);
        }

        // PUT: api/v1/amenities/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateAmenityDto dto)
        {
            var updated = await _amenityService.UpdateAsync(id, dto);
            if (!updated) return NotFound(new ApiErrorResponse { 
                Status = 404, 
                Message = "Amenidad no encontrada.", 
                Error = null });

            return NoContent();
        }

        // DELETE: api/v1/amenities/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _amenityService.DeleteAsync(id);
            if (!deleted) return NotFound(new ApiErrorResponse { Status = 404, Message = "Amenidad no encontrada.", Error = null });

            return NoContent();
        }
    }

}
