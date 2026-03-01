using Hotel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Infrastructure.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ToTable("reservations");
            builder.HasKey(r => r.ReservationId);

            builder.Property(r => r.ReservationId).HasColumnName("reservation_id");
            builder.Property(r => r.RoomId).HasColumnName("room_id").IsRequired();
            builder.Property(r => r.CustomerId).HasColumnName("customer_id").IsRequired();
            builder.Property(r => r.CheckInDate).HasColumnName("check_in_date").HasColumnType("date").IsRequired();
            builder.Property(r => r.CheckOutDate).HasColumnName("check_out_date").HasColumnType("date").IsRequired();
            builder.Property(r => r.ReservationStatusId).HasColumnName("reservation_status_id").IsRequired();
            builder.Property(r => r.PaymentStatusId).HasColumnName("payment_status_id").IsRequired();
            builder.Property(r => r.TotalAmount).HasColumnName("total_amount").HasColumnType("numeric(18,2)").IsRequired();
            builder.Property(r => r.PaidAmount).HasColumnName("paid_amount").HasColumnType("numeric(18,2)").IsRequired();
            builder.Property(r => r.ReservationCode).HasColumnName("reservation_code").HasMaxLength(6).IsRequired();

            // Campos de auditoría / Borrado lógico
            builder.Property(r => r.IsActive).HasColumnName("is_active").IsRequired()
                   .HasDefaultValueSql("true");
            builder.Property(r => r.CreatedBy).HasColumnName("create_by").HasMaxLength(50).IsRequired();
            builder.Property(r => r.CreatedDate).HasColumnName("created_date")
                .HasConversion(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            builder.Property(r => r.UpdatedDate).HasColumnName("updated_date")
                .HasConversion(
                    v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null);

            // Relaciones
            builder.HasOne(r => r.Room)
                   .WithMany()
                   .HasForeignKey(r => r.RoomId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Customer)
                   .WithMany()
                   .HasForeignKey(r => r.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.ReservationStatus)
                   .WithMany()
                   .HasForeignKey(r => r.ReservationStatusId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.PaymentStatus)
                   .WithMany()
                   .HasForeignKey(r => r.PaymentStatusId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
