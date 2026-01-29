using Hotel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Infrastructure.Configurations
{
    public class PaymentStatusConfiguration : IEntityTypeConfiguration<PaymentStatus>
    {
        public void Configure(EntityTypeBuilder<PaymentStatus> builder)
        {
            builder.ToTable("payment_statuses");
            builder.HasKey(x => x.PaymentStatusId);
            builder.Property(x => x.PaymentStatusId)
                .HasColumnName("payment_status_id");
            builder.Property(x => x.Code)
                .HasColumnName("code")
                .HasMaxLength(20)
                .IsRequired();
            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(x => x.IsFinal)
                .HasColumnName("is_final");
            // Auditoría mappings
            builder.Property(x => x.CreatedBy)
                .HasColumnName("created_by")
                .HasMaxLength(50);
            builder.Property(x => x.CreatedDate)
                .HasColumnName("created_date");
            builder.Property(x => x.IsActive)
                .HasColumnName("is_active");

            builder.HasIndex(x => x.Code)
                .IsUnique();
        }
    }
}
