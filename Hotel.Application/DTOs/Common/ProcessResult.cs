namespace Hotel.Application.DTOs.Common
{
    public class ProcessResult<T>
    {
        public bool Succeeded { get; private set; }
        public string? Message { get; private set; }
        public T? Data { get; private set; }

        private ProcessResult(bool succeeded, string? message, T? data)
        {
            Succeeded = succeeded;
            Message = message;
            Data = data;
        }

        public static ProcessResult<T> Success(T data, string? message = null) 
            => new ProcessResult<T>(true, message, data);

        public static ProcessResult<T> Failure(string message) 
            => new ProcessResult<T>(false, message, default);
    }
}
