using Decongestor.Domain;
using Microsoft.EntityFrameworkCore;

namespace Decongestor.DataAccess.Config
{
    public class DataContext : DbContext
    {
        public DbSet<VehicleType> VehicleTypes { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<ChargeMatrix> ChargeMatrix { get; set; }

        public DbSet<TollEntry> TollEntries { get; set; }

        public DbSet<HolidayConfiguration> HolidayConfiguration { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new VehicleTypeEntityTypeConfig());
            modelBuilder.ApplyConfiguration(new VehicleEntityTypeConfig());
            modelBuilder.ApplyConfiguration(new ChargeMatrixEntityTypeConfig());
            modelBuilder.ApplyConfiguration(new TollEntryEntityTypeConfig());
            modelBuilder.ApplyConfiguration(new HolidayConfigurationEntityTypeConfig());
        }
    }
}
