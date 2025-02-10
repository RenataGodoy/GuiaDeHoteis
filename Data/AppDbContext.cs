using Microsoft.EntityFrameworkCore;
using GuiaDeMoteisAPI.Models; // Importando o namespace Models

namespace GuiaDeMoteisAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Suite> Suites { get; set; }
        public DbSet<Motel> Motels { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<User> Users { get; set; } 


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Client)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.ClientId);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Suite)
                .WithMany(s => s.Reservations)
                .HasForeignKey(r => r.SuiteId);

            modelBuilder.Entity<Suite>()
                .HasOne(s => s.Motel)
                .WithMany(m => m.Suites)
                .HasForeignKey(s => s.MotelId);
         
            base.OnModelCreating(modelBuilder);
        }
    }
}
