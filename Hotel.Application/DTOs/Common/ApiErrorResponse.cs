namespace Hotel.Application.DTOs.Common
{
    public class ApiErrorResponse
    {
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Error { get; set; }
    }
}
