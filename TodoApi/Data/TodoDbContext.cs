using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Todo> Todoes { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Session> Sessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder
                        .Entity(entityType.ClrType)
                        .Property(nameof(AuditableEntity.CreatedAt))
                        .HasDefaultValueSql("NOW()");

                    modelBuilder
                        .Entity(entityType.ClrType)
                        .Property(nameof(AuditableEntity.UpdatedAt))
                        .HasDefaultValueSql("NOW()");
                }
            }
            base.OnModelCreating(modelBuilder);
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
            var entries = ChangeTracker
                .Entries<AuditableEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var now = DateTime.UtcNow;
                entry.Entity.UpdatedAt = now;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                }
            }
        }
    }
}
