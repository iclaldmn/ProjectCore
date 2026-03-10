using Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Context;

public partial class AppDbContext
{
    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var userId = _httpContextAccessor.HttpContext?
            .User?
            .FindFirst(ClaimTypes.NameIdentifier)?
            .Value;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is BaseEntity baseEntity &&
                entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                baseEntity.Silindi = true;
            }

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