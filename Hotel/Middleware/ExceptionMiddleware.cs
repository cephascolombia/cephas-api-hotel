using Hotel.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace Hotel.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Intenta seguir con la petición normal
                await _next(context);
            }
            catch (BusinessException ex) // Atrapa específicamente tus errores de negocio
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            }
            catch (Exception ex) // Atrapa cualquier otro error inesperado (bugs, caídas de BD)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new Hotel.Application.DTOs.Common.ApiErrorResponse
            {
                Status = context.Response.StatusCode,
                Message = exception.Message,
                Error = exception is BusinessException ? null : "Ocurrió un error interno en el servidor."
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}
