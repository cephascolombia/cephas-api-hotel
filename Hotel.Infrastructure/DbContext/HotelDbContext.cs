using Hotel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Hotel.Domain.Common;

namespace Hotel.Infrastructure.DbContext
{
    public class HotelDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<Amenity> Amenities => Set<Amenity>();
        public DbSet<ReservationStatus> ReservationStatuses => Set<ReservationStatus>();
        public DbSet<PaymentStatus> PaymentStatuses => Set<PaymentStatus>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<Payment> Payments => Set<Payment>();

        // Sobrescribimos el método de guardado para que CreateDate sea automático en tablas
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is IAuditableEntity && e.State == EntityState.Added);

            foreach (var entityEntry in entries)
            {
                ((IAuditableEntity)entityEntry.Entity).CreatedDate = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
        //
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(HotelDbContext).Assembly
            );

            //==========================================================
            //Filtros globales, automáticamente filtra IsActive = true
            //==========================================================
            // Customers
            modelBuilder.Entity<Customer>()
                .HasQueryFilter(c => c.IsActive);

            // Rooms
            modelBuilder.Entity<Room>()
                .HasQueryFilter(r => r.IsActive);

            // Amenities
            modelBuilder.Entity<Amenity>()
                .HasQueryFilter(a => a.IsActive);

            // ReservationStatuses
            modelBuilder.Entity<ReservationStatus>().HasQueryFilter(x => x.IsActive);

            // PaymentStatus
            modelBuilder.Entity<PaymentStatus>().HasQueryFilter(x => x.IsActive);

            // Reservations
            modelBuilder.Entity<Reservation>()
                .HasQueryFilter(r => r.IsActive);

            // Payments
            modelBuilder.Entity<Payment>()
                .HasQueryFilter(p => p.IsActive);

            // Configura todas las propiedades DateTime para que siempre sean UTC
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?));

                foreach (var property in properties)
                {
                    property.SetValueConverter(new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime, DateTime>(
                        v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)));
                }
            }
        }
    }
}
