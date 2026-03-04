using Domain.Common;
using Domain.Entities.Kullanici;
using Domain.Entities.Ortak;
using Domain.Entities.ProjeModul;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;

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
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly
        );

        // 🔥 Global Soft Delete Filter
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(AppDbContext)
                    .GetMethod(nameof(SetSoftDeleteFilter),
                        BindingFlags.NonPublic | BindingFlags.Static)
                    ?.MakeGenericMethod(entityType.ClrType);

                method?.Invoke(null, new object[] { modelBuilder });
            }
        }
    }

    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private static void SetSoftDeleteFilter<TEntity>(
        ModelBuilder builder)
        where TEntity : BaseEntity
    {
        builder.Entity<TEntity>()
            .HasQueryFilter(e => !e.Silindi);
    }

    public override async Task<int> SaveChangesAsync(
    CancellationToken cancellationToken = default)
    {
        var userId = _httpContextAccessor.HttpContext?
            .User?
            .FindFirst(ClaimTypes.NameIdentifier)?
            .Value;

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
                    if (long.TryParse(userId, out var uid))
                        history.OlusturanKullanici = uid;
                }
                else if (entry.State == EntityState.Modified)
                {
                    history.GuncellemeZamani = DateTime.UtcNow;
                    if (long.TryParse(userId, out var uid))
                        history.GuncelleyenKullanici = uid;
                }
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}