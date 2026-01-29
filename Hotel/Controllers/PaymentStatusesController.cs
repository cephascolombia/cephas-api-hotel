using Hotel.Application.DTOs.Common;
using Hotel.Application.DTOs.PaymentStatuses;
using Hotel.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/payment-statuses")]
    [ApiController]
    public class PaymentStatusesController : ControllerBase
    {
        private readonly IPaymentStatusService _statusService;

        public PaymentStatusesController(IPaymentStatusService statusService)
        {
            _statusService = statusService;
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResponseDto<PaymentStatusDto>>> GetPaged([FromQuery] PagedRequestDto request)
        {
            return Ok(await _statusService.GetPagedAsync(request));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentStatusDto>> GetById(int id)
        {
            var result = await _statusService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreatePaymentStatusDto dto)
        {
            var id = await _statusService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreatePaymentStatusDto dto)
        {
            var updated = await _statusService.UpdateAsync(id, dto);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _statusService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}