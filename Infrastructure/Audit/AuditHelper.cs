using Domain.Entities.Log;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Audit;

public static class AuditHelper
{
    public static List<AuditLog> CreateAuditLogs(
        ChangeTracker changeTracker,
        string userId,
        string userName)
    {

        var auditLogs = new List<AuditLog>();

        var entries = changeTracker.Entries()
            .Where(e => !(e.Entity is AuditLog))
            .ToList();

        foreach (var entry in entries)
        {
            var entityName = entry.Entity.GetType().Name;

            var entityId = entry.Properties
                .FirstOrDefault(p => p.Metadata.IsPrimaryKey())
                ?.CurrentValue?.ToString() ?? "0";

            // CREATE
            if (entry.State == EntityState.Added)
            {
                var newValues = entry.Properties.ToDictionary(
                    p => p.Metadata.Name,
                    p => p.CurrentValue
                );

                auditLogs.Add(new AuditLog
                {
                    UserId = userId,
                    UserName = userName,
                    Action = "Create",
                    EntityName = entityName,
                    EntityId = entityId,
                    Changes = JsonSerializer.Serialize(new
                    {
                        action = "create",
                        entityId,
                        @new = newValues
                    }),
                    CreatedAt = DateTime.UtcNow
                });
            }

            // UPDATE
            else if (entry.State == EntityState.Modified)
            {
                var changesDict = new Dictionary<string, object>();

                foreach (var property in entry.Properties)
                {
                    if (!Equals(property.OriginalValue, property.CurrentValue))
                    {
                        changesDict[property.Metadata.Name] = new
                        {
                            old = property.OriginalValue,
                            @new = property.CurrentValue
                        };
                    }
                }

                if (changesDict.Any())
                {
                    auditLogs.Add(new AuditLog
                    {
                        UserId = userId,
                        UserName = userName,
                        Action = "Update",
                        EntityName = entityName,
                        EntityId = entityId,
                        Changes = JsonSerializer.Serialize(new
                        {
                            action = "update",
                            entityId,
                            changes = changesDict
                        }),
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }
        }

        return auditLogs;
    }
}
