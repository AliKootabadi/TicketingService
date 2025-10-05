using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TicketingService.EntityFramework.Models;

namespace TicketingService.EntityFramework
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Ticket> Tickets => Set<Ticket>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(e =>
            {
                e.HasKey(u => u.Id);
                e.HasIndex(u => u.Email).IsUnique();
                e.Property(u => u.Role).HasConversion<string>().HasMaxLength(20);
            });

            builder.Entity<Ticket>(e =>
            {
                e.HasKey(t => t.Id);
                e.Property(t => t.Status).HasConversion<string>();
                e.Property(t => t.Priority).HasConversion<string>();

                e.Property(t => t.CreatedAt);

                e.Property(t => t.UpdatedAt);

                e.HasOne(t => t.CreatedByUser)
                 .WithMany(u => u.CreatedTickets)
                 .HasForeignKey(t => t.CreatedByUserId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(t => t.AssignedToUser)
                 .WithMany(u => u.AssignedTickets)
                 .HasForeignKey(t => t.AssignedToUserId)
                 .OnDelete(DeleteBehavior.SetNull);
            });
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries<Ticket>();
            var Now = DateTime.Now;
            foreach (var e in entries)
            {
                if (e.State == EntityState.Added)
                {
                    e.Entity.CreatedAt = Now;
                    e.Entity.UpdatedAt = Now;
                }
                else if (e.State == EntityState.Modified)
                {
                    e.Entity.UpdatedAt = Now;
                }
            }
        }
    }

}
