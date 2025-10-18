using Microsoft.EntityFrameworkCore;
using AccesoDatos.Models;

namespace AccesoDatos.Data
{
    public class AccesoDatosDbContext : DbContext
    {
        public AccesoDatosDbContext(DbContextOptions<AccesoDatosDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<ProjectImage> ProjectImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .Property(p => p.Technologies)
                .HasConversion(
                    v => string.Join(';', v),
                    v => v.Split(';', StringSplitOptions.RemoveEmptyEntries)
                );

            // Configurar clave foránea opcional de ContactMessage a User
            modelBuilder.Entity<ContactMessage>()
                .HasOne(cm => cm.User)
                .WithMany()
                .HasForeignKey(cm => cm.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}