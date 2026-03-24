using Domain.Entities.Kullanici;
using Domain.Entities.Log;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public partial class AppDbContext
    : IdentityDbContext<AppUser, AppRole, long>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>()
        .HasMany<IdentityUserRole<long>>(u => u.UserRoles)
        .WithOne()
        .HasForeignKey(ur => ur.UserId)
        .IsRequired();


        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly
        );

        ApplySoftDelete(modelBuilder); // 🔥 EKLEDİĞİN YER
    }


    private List<AuditLog> GetAuditLogs()
    {
        ChangeTracker.DetectChanges();

        var auditLogs = new List<AuditLog>();

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State != EntityState.Modified)
                continue;

            var entityName = entry.Entity.GetType().Name;

            var changes = new List<string>();

            foreach (var property in entry.Properties)
            {
                var original = property.OriginalValue;
                var current = property.CurrentValue;

                if (!Equals(original, current))
                {
                    changes.Add(
                        $"{property.Metadata.Name}: {original} → {current}");
                }
            }

            if (changes.Any())
            {
                auditLogs.Add(new AuditLog
                {
                    EntityName = entityName,
                    Action = "Update",
                    Changes = string.Join(", ", changes),
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        return auditLogs;
    }
}