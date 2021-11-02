using grpcArachne.Models;
using Microsoft.EntityFrameworkCore;

namespace grpcArachne.Data
{
    public class ServerDbContext : DbContext
    {
        public ServerDbContext(DbContextOptions options) : base(options) { }
        public DbSet<DbT0Perangkat> T0PerangkatDbSet { get; set; }
        public DbSet<DbT1DataSensor> T1DataSensorDbSet { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbT0Perangkat>(entity =>
            {
                entity.ToTable("T0Perangkat");
                entity.HasKey(e => e.IdPerangkat);
            });

            modelBuilder.Entity<DbT1DataSensor>(entity =>
            {
                entity.ToTable("T1DataSensor");
                entity.HasKey(e => e.IdDataSensor);
            });
        }
    }
}