namespace Hotel.Application.DTOs.Common
{
    public class PagedRequestDto
    {
        public int Page { get; set; } = 1;
        public int PageNumber { get => Page; set => Page = value; }
        public int PageSize { get; set; } = 10;
        // Filtros comunes
        public string? Search { get; set; }
        public int Skip => (Page - 1) * PageSize;
    }
}
