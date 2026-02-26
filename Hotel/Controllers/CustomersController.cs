using Hotel.Application.DTOs.Common;
using Hotel.Application.DTOs.Customers;
using Hotel.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/customers")]
    public class CustomersController : ApiControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
            => _customerService = customerService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] PagedRequestDto request)
        {
            var result = await _customerService.GetPagedAsync(request);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound(new ApiErrorResponse 
                { 
                    Status = 404, 
                    Message = "Cliente no encontrado.", 
                    Error = null 
                });
            }

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
        {
            var result = await _customerService.CreateAsync(dto);

            if (!result.Succeeded)
                return HandleFailure(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data }, dto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerDto dto)
        {
            if (dto.CustomerId != 0 && id != dto.CustomerId)
            {
                return BadRequest(new ApiErrorResponse
                {
                    Status = 400,
                    Message = "El ID del cliente en la URL no coincide con el ID del cuerpo de la solicitud.",
                    Error = null
                });
            }

            var result = await _customerService.UpdateAsync(id, dto);

            if (!result.Succeeded)
                return HandleFailure(result);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _customerService.DeleteAsync(id);

            if (!result.Succeeded)
                return HandleFailure(result);

            return NoContent();
        }
    }
}