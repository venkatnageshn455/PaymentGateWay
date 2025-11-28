using Microsoft.EntityFrameworkCore;
using PaymentsBackend.DataModels;

namespace PaymentsBackend.DataStore
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<DailySequence> DailySequences { get; set; } = null!;
        public DbSet<PaymentLog> PaymentLogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Payment>()
                .HasIndex(p => new { p.UserId, p.ClientRequestId })
                .IsUnique();

           
            modelBuilder.Entity<DailySequence>()
                .HasIndex(s => new { s.UserId, s.Date })
                .IsUnique();

           
            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.Reference);
        }
    }
}
