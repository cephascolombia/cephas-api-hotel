using Hotel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Infrastructure.Configurations
{
    public class AmenityConfiguration : IEntityTypeConfiguration<Amenity>
    {
        public void Configure(EntityTypeBuilder<Amenity> builder)
        {
            builder.ToTable("amenities");
            builder.HasKey(a => a.AmenityId);
            builder.Property(a => a.AmenityId)
                .HasColumnName("amenity_id");
            builder.Property(a => a.Name)
                .HasColumnName("name")
                .HasMaxLength(50).IsRequired();
            builder.Property(a => a.CreatedBy)
                .HasColumnName("created_by")
                .HasMaxLength(50).IsRequired();
            builder.Property(a => a.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);
            builder.Property(a => a.CreatedDate)
                .HasColumnName("created_date")
                   .HasConversion(v => v.ToUniversalTime(), 
                   v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            builder.HasIndex(a => a.Name).IsUnique();
        }
    }
}
