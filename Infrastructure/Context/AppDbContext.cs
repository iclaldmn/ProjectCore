using Domain.Common;
using Domain.Entities.Kullanici;
using Domain.Entities.ProjeModul;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public partial class AppDbContext
    : IdentityDbContext<AppUser, IdentityRole<long>, long>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            // 🔹 Soft Delete
            if (entry.Entity is BaseEntity baseEntity &&
                entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                baseEntity.Silindi = true;
            }

            // 🔹 Audit
            if (entry.Entity is HistoryEntity history)
            {
                if (entry.State == EntityState.Added)
                {
                    history.OlusturmaZamani = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    history.GuncellemeZamani = DateTime.UtcNow;
                }
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}