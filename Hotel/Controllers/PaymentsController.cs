using Hotel.Application.DTOs.Common;
using Hotel.Application.DTOs.Payments;
using Hotel.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/payments")]
    public class PaymentsController : ApiControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // GET: api/v1/payments/paged
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] PagedRequestDto request)
        {
            var result = await _paymentService.GetPagedAsync(request);
            return Ok(result);
        }

        // GET: api/v1/payments/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            if (payment == null) return NotFound(new ApiErrorResponse { Status = 404, Message = "Pago no encontrado.", Error = null });

            return Ok(payment);
        }

        // POST: api/v1/payments
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentDto dto)
        {
            var paymentId = await _paymentService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = paymentId }, dto);
        }

        // PUT: api/v1/payments/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreatePaymentDto dto)
        {
            var updated = await _paymentService.UpdateAsync(id, dto);
            if (!updated) return NotFound(new ApiErrorResponse { Status = 404, Message = "Pago no encontrado.", Error = null });

            return NoContent();
        }

        // DELETE: api/v1/payments/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _paymentService.DeleteAsync(id);
            if (!deleted) return NotFound(new ApiErrorResponse { Status = 404, Message = "Pago no encontrado.", Error = null });

            return NoContent();
        }
    }
}
