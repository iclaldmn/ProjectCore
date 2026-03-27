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

        private (string userId, string userName) GetUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = user?.Identity?.Name ?? "System";

            return (userId, userName);
        }

        public async Task LogCreateAsync(
            string entityName,
            string entityId,
            object newValues)
        {
            var (userId, userName) = GetUser();

            var log = new AuditLog
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
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task LogUpdateAsync(
            string entityName,
            string entityId,
            object changes)
        {
            var (userId, userName) = GetUser();

            var log = new AuditLog
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
                    changes
                }),
                CreatedAt = DateTime.UtcNow
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task LogDeleteAsync(
            string entityName,
            string entityId,
            object oldValues)
        {
            var (userId, userName) = GetUser();

            var log = new AuditLog
            {
                UserId = userId,
                UserName = userName,
                Action = "Delete",
                EntityName = entityName,
                EntityId = entityId,
                Changes = JsonSerializer.Serialize(new
                {
                    action = "delete",
                    entityId,
                    old = oldValues
                }),
                CreatedAt = DateTime.UtcNow
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }