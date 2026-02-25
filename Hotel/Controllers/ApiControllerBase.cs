using Hotel.Application.DTOs.Common;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected IActionResult HandleFailure<T>(ProcessResult<T> result)
        {
            var errorResponse = new ApiErrorResponse
            {
                Status = 400,
                Message = result.Message ?? "Ocurrió un error en la solicitud.",
                Error = null
            };

            // Detectar 404 basado en el mensaje (estándar en este proyecto)
            if (result.Message?.Contains("no encontrado") == true)
            {
                errorResponse.Status = 404;
                return NotFound(errorResponse);
            }

            return BadRequest(errorResponse);
        }
    }
}
