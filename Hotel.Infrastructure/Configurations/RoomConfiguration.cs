using Hotel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Infrastructure.Configurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.ToTable("rooms");

            builder.HasKey(r => r.RoomId);

            builder.Property(r => r.RoomId)
                   .HasColumnName("room_id");

            builder.Property(r => r.Name)
                   .HasColumnName("name")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(r => r.PricePerNight)
                   .HasColumnName("price_per_night")
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(r => r.Capacity)
                   .HasColumnName("capacity")
                   .IsRequired();

            builder.Property(r => r.Description)
                   .HasColumnName("description")
                   .HasMaxLength(500);

            builder.Property(r => r.CreatedBy) 
                   .HasColumnName("created_by")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(r => r.IsActive)
                   .HasColumnName("is_active")
                   .HasDefaultValue(true)
                   .IsRequired();

            builder.Property(r => r.IsWorking)
                   .HasColumnName("is_working")
                   .HasDefaultValue(true)
                   .IsRequired();

            builder.Property(r => r.CreatedDate)
                   .HasColumnName("created_date")
                   .HasConversion(
                       v => v.ToUniversalTime(),
                       v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                   )
                   .IsRequired();
        }
    }
}