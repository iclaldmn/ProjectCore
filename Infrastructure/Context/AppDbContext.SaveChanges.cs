using Domain.Common;
using Domain.Entities.Log;
using Infrastructure.Audit;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace Infrastructure.Context;

public partial class AppDbContext
{
    public override async Task<int> SaveChangesAsync(
    CancellationToken cancellationToken = default)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        var userId = user?
            .FindFirst(ClaimTypes.NameIdentifier)?
            .Value;

        var userName = user?
            .Identity?
            .Name ?? "System"; // 🔥 FIX

        foreach (var entry in ChangeTracker.Entries())
        {
            // 🔥 SOFT DELETE (KALDI)
            if (entry.Entity is BaseEntity baseEntity &&
                entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                baseEntity.Silindi = true;
            }

            // 🔥 HISTORY (KALDI)
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

        // 🔥 AUDIT OLUŞTUR
        var auditLogs = AuditHelper.CreateAuditLogs(
            ChangeTracker,
            userId,
            userName
        );

        if (auditLogs.Any())
        {
            AuditLogs.AddRange(auditLogs);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
    

}