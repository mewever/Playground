using Microsoft.EntityFrameworkCore;

namespace RazorShipBuilder.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<MountDevice> MountDevices { get; set; }
        public DbSet<MountPoint> MountPoints { get; set; }
        public DbSet<Starship> Starships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ssbuild");
            base.OnModelCreating(modelBuilder);
        }
    }
}
