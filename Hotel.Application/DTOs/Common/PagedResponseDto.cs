namespace Hotel.Application.DTOs.Common
{
    public class PagedResponseDto<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public int TotalRecords { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
