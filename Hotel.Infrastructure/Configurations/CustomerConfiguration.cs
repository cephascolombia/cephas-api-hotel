using Hotel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Infrastructure.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("customers");

            builder.HasKey(c => c.CustomerId);

            builder.Property(c => c.CustomerId)
                   .HasColumnName("customer_id");

            builder.Property(c => c.FullName)
                   .HasColumnName("full_name")
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(c => c.Email)
                   .HasColumnName("email")
                   .HasMaxLength(150)
                   .IsRequired();

            builder.HasIndex(c => c.Email)
                   .IsUnique();

            builder.Property(c => c.Phone)
                   .HasColumnName("phone")
                   .HasMaxLength(20);

            builder.Property(c => c.IdentityDocument)
                   .HasColumnName("identity_document")
                   .HasMaxLength(30);

            builder.Property(c => c.Address)
                   .HasColumnName("address")
                   .HasMaxLength(200);

            builder.Property(c => c.Notes)
                   .HasColumnName("notes")
                   .HasMaxLength(500);

            builder.Property(c => c.CreatedDate)
                   .HasColumnName("created_date")
                   .HasConversion(
                       v => v.ToUniversalTime(),
                       v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                   )
                   .IsRequired();

            builder.Property(c => c.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(c => c.IsActive)
                   .HasColumnName("is_active")
                   .HasDefaultValue(true)
                   .IsRequired();
        }
    }
}
