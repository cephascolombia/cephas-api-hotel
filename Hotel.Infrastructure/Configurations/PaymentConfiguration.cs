using Hotel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Infrastructure.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("payments");
            builder.HasKey(p => p.PaymentId);

            builder.Property(p => p.PaymentId).HasColumnName("payment_id");
            builder.Property(p => p.ReservationId).HasColumnName("reservation_id").IsRequired();
            builder.Property(p => p.Amount).HasColumnName("amount").HasColumnType("numeric(18,2)").IsRequired();
            builder.Property(p => p.PaymentDate).HasColumnName("payment_date")
                .HasConversion(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            builder.Property(p => p.PaymentMethod).HasColumnName("payment_method").HasMaxLength(50).IsRequired();
            builder.Property(p => p.Reference).HasColumnName("reference").HasMaxLength(100);

            // Campos de auditoría / Borrado lógico
            builder.Property(p => p.IsActive).HasColumnName("is_active").IsRequired()
                   .HasDefaultValueSql("true");
            builder.Property(p => p.CreatedBy).HasColumnName("create_by").HasMaxLength(50).IsRequired();

            // Relaciones
            builder.HasOne(p => p.Reservation)
                   .WithMany()
                   .HasForeignKey(p => p.ReservationId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
