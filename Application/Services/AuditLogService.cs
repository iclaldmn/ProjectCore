using Domain.Entities.Log;
using Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services;

public class AuditLogService : IAuditLogService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditLogService(
        AppDbContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task LogAsync(
        string action,
        string entityName,
        string entityId)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = user?.Identity?.Name;

        var log = new AuditLog
        {
            UserId = userId,
            UserName = userName,
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            Changes = "{}",
            CreatedAt = DateTime.UtcNow
        };

        _context.AuditLogs.Add(log);

        await _context.SaveChangesAsync();
    }

    // 🔥 GENERIC CREATE (asıl kullanacağın)
    public async Task LogCreateAsync(
    string entityName,
    string entityId)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = user?.Identity?.Name;

        var createJson = new
        {
            type = "create"
        };

        var log = new AuditLog
        {
            UserId = userId,
            UserName = userName ?? "System",
            Action = "Create",
            EntityName = entityName,
            EntityId = entityId,
            Changes = JsonSerializer.Serialize(createJson),
            CreatedAt = DateTime.UtcNow
        };

        _context.AuditLogs.Add(log);
        await _context.SaveChangesAsync();
    }



}