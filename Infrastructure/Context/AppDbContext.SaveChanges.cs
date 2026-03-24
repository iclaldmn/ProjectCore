using Domain.Common;
using Domain.Entities.Log;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace Infrastructure.Context;

public partial class AppDbContext
{
    //public override async Task<int> SaveChangesAsync(
    //    CancellationToken cancellationToken = default)
    //{
    //    var auditLogs = new List<AuditLog>();
    //    var user = _httpContextAccessor.HttpContext?.User;

    //    var userId = user?
    //        .FindFirst(ClaimTypes.NameIdentifier)?
    //        .Value;

    //    var userName = user?
    //        .Identity?
    //        .Name;

    //    foreach (var entry in ChangeTracker.Entries())
    //    {
    //        if (entry.State == EntityState.Modified)
    //        {
    //            var changes = new List<string>();

    //            foreach (var property in entry.Properties)
    //            {
    //                var original = property.OriginalValue;
    //                var current = property.CurrentValue;

    //                if (!Equals(original, current))
    //                {
    //                    changes.Add($"{property.Metadata.Name}: {original} → {current}");
    //                }
    //            }

    //            if (changes.Any())
    //            {
    //                auditLogs.Add(new AuditLog
    //                {
    //                    UserId = userId,
    //                    Action = "Update",
    //                    EntityName = entry.Entity.GetType().Name,
    //                    EntityId = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue?.ToString(),
    //                    Changes = string.Join(", ", changes),
    //                    CreatedAt = DateTime.UtcNow
    //                });
    //            }
    //        }

    //        //if (entry.Entity is BaseEntity baseEntity &&
    //        //    entry.State == EntityState.Deleted)
    //        //{
    //        //    entry.State = EntityState.Modified;
    //        //    baseEntity.Silindi = true;
    //        //}

    //        //if (entry.Entity is HistoryEntity history)
    //        //{
    //        //    if (entry.State == EntityState.Added)
    //        //    {
    //        //        history.OlusturmaZamani = DateTime.UtcNow;

    //        //        if (long.TryParse(userId, out var uid))
    //        //            history.OlusturanKullanici = uid;
    //        //    }
    //        //    else if (entry.State == EntityState.Modified)
    //        //    {
    //        //        history.GuncellemeZamani = DateTime.UtcNow;

    //        //        if (long.TryParse(userId, out var uid))
    //        //            history.GuncelleyenKullanici = uid;
    //        //    }
    //        //}
    //        }
    //        //return await base.SaveChangesAsync(cancellationToken);
    //        var result = await base.SaveChangesAsync(cancellationToken);

    //        if (auditLogs.Any())
    //        {
    //            AuditLogs.AddRange(auditLogs);
    //            await base.SaveChangesAsync(cancellationToken);
    //        }

    //        return result;
    //    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var auditLogs = new List<AuditLog>();

        var user = _httpContextAccessor.HttpContext?.User;

        var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = user?.Identity?.Name ?? "System";

        var allChanges = new Dictionary<string, object>();

        var entries = ChangeTracker.Entries()
            .Where(e => !(e.Entity is AuditLog))
            .ToList();

        foreach (var entry in entries)
        {
            var entityName = entry.Entity.GetType().Name;

            // 🔹 Soft Delete
            if (entry.Entity is BaseEntity baseEntity &&
                entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                baseEntity.Silindi = true;
            }

            var entityId = entry.Properties
                .FirstOrDefault(p => p.Metadata.IsPrimaryKey())
                ?.CurrentValue?.ToString();

            // 🔥 CREATE
            if (entry.State == EntityState.Added)
            {
                var newValues = new Dictionary<string, object>();

                foreach (var property in entry.Properties)
                {
                    newValues[property.Metadata.Name] = property.CurrentValue;
                }

                allChanges[entityName] = new
                {
                    action = "create",
                    entityId,
                    @new = newValues
                };
            }

            // 🔥 MODIFIED (UPDATE veya DELETE ayrımı burada)
            else if (entry.State == EntityState.Modified)
            {
                var changedProperties = entry.Properties
                    .Where(p => !Equals(p.OriginalValue, p.CurrentValue))
                    .ToList();

                // 🔥 GERÇEK DELETE (SADECE Silindi değişmişse)
                if (changedProperties.Count == 1 &&
                    changedProperties.Any(p => p.Metadata.Name == "Silindi"))
                {
                    var oldValues = new Dictionary<string, object>();

                    foreach (var property in entry.Properties)
                    {
                        oldValues[property.Metadata.Name] = property.OriginalValue;
                    }

                    allChanges[entityName] = new
                    {
                        action = "delete",
                        entityId,
                        old = oldValues
                    };

                    continue;
                }

                // 🔥 UPDATE
                var changesDict = new Dictionary<string, object>();

                foreach (var property in changedProperties)
                {
                    changesDict[property.Metadata.Name] = new
                    {
                        old = property.OriginalValue,
                        @new = property.CurrentValue
                    };
                }

                if (changesDict.Any())
                {
                    allChanges[entityName] = new
                    {
                        action = "update",
                        entityId,
                        changes = changesDict
                    };
                }
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        if (allChanges.Any())
        {
            auditLogs.Add(new AuditLog
            {
                UserId = userId,
                UserName = userName,
                Action = "Bulk",
                EntityName = "Multiple",
                EntityId = "MULTI",
                Changes = JsonSerializer.Serialize(allChanges),
                CreatedAt = DateTime.UtcNow
            });

            AuditLogs.AddRange(auditLogs);
            await base.SaveChangesAsync(cancellationToken);
        }

        return result;
    }
}