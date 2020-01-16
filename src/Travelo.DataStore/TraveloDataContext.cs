using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using Travelo.Core.Domain;

namespace Travelo.DataStore
{
    public class TraveloDataContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<CustomerTrip> CustomerTrips { get; set; }
        private readonly SqlSettings _sqlSettings;

        public TraveloDataContext(DbContextOptions<TraveloDataContext> options, IOptions<SqlSettings> sqlSettings) :
            base(options)
        {
            _sqlSettings = sqlSettings.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //TODO: create in memory Travelo DB Context for end to end testing, and inject it into startup, for now I leave it as it is.
            if (_sqlSettings.InMemory)
            {
                optionsBuilder.UseInMemoryDatabase("Travelo");
                return;
            }
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_sqlSettings.DefaultConnection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasKey(x => new {x.Id});
            modelBuilder.Entity<Customer>().HasIndex(x => x.FullName);
            modelBuilder.Entity<Customer>().Property(p => p.FirstName).IsRequired().HasMaxLength(128);
            modelBuilder.Entity<Customer>().Property(p => p.LastName).IsRequired().HasMaxLength(128);
            modelBuilder.Entity<Customer>().Property(p => p.FullName).IsRequired().HasMaxLength(128);

            modelBuilder.Entity<Trip>().HasKey(x => new {x.Id});
            modelBuilder.Entity<Trip>().HasIndex(x => x.Name);
            modelBuilder.Entity<Trip>().Property(p => p.StartDateUTC).IsRequired();
            modelBuilder.Entity<Trip>().Property(p => p.EndDateUTC).IsRequired();
            modelBuilder.Entity<Trip>().Property(p => p.TripStatus).IsRequired();
            modelBuilder.Entity<Trip>().Property(e => e.TripStatus).IsRequired().HasMaxLength(128)
                .HasConversion(new EnumToStringConverter<TripStatus>());
            modelBuilder.Entity<Trip>().Property(e => e.Name).IsRequired().HasMaxLength(256);
            modelBuilder.Entity<Trip>().Property(e => e.Description).HasMaxLength(2048);
            modelBuilder.Entity<Trip>().Property(e => e.TripDestination).IsRequired().HasMaxLength(256);

            modelBuilder.Entity<CustomerTrip>().HasKey(x => new {x.CustomerId, x.TripId});
            modelBuilder.Entity<CustomerTrip>().HasOne(p => p.Customer).WithMany(p => p.CustomerTrips)
                .HasForeignKey(p => p.CustomerId);
            modelBuilder.Entity<CustomerTrip>().HasOne(p => p.Trip).WithMany(p => p.CustomerTrips)
                .HasForeignKey(p => p.TripId);
            base.OnModelCreating(modelBuilder);
        }
    }
}