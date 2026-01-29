namespace Hotel.Domain.Common
{
    public interface IAuditableEntity
    {
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
    }
}
