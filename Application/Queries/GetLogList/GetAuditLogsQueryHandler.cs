using Application.DTOs.LogDto;
using Application.Helpers;
using Infrastructure.Context;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Queries.GetLogList;

public class GetAuditLogsQueryHandler
    : IRequestHandler<GetAuditLogsQuery, Result<List<AuditLogDto>>>
{
    private readonly AppDbContext _context;

    public GetAuditLogsQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<AuditLogDto>>> Handle(
        GetAuditLogsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.AuditLogs.AsQueryable();

        // 🔍 filtreler
        if (!string.IsNullOrWhiteSpace(request.UserName))
            query = query.Where(x => x.UserName == request.UserName);

        if (!string.IsNullOrWhiteSpace(request.Action))
            query = query.Where(x => x.Action == request.Action);

        // 🔥 DB'den çek
        var data = await query
            .OrderByDescending(x => x.CreatedAt)
            .Take(request.Take)
            .ToListAsync(cancellationToken);

        // 🔥 MEMORY’de parse (çok önemli)
        //var result = data.Select(x => new AuditLogDto
        //{
        //    Id = x.Id,
        //    UserName = x.UserName,
        //    Action = x.Action,
        //    EntityName = x.EntityName,
        //    EntityId = x.EntityId,
        //    CreatedAt = x.CreatedAt,
        //    Changes = string.IsNullOrEmpty(x.Changes)
        //        ? null
        //        : JsonSerializer.Deserialize<object>(x.Changes)
        //}).ToList();

        var result = data.Select(x =>
        {
            object? parsedChanges = null;

            if (!string.IsNullOrWhiteSpace(x.Changes))
            {
                try
                {
                    parsedChanges = JsonSerializer.Deserialize<object>(x.Changes);
                }
                catch
                {
                    parsedChanges = x.Changes;
                }
            }

            return new AuditLogDto
            {
                Id = x.Id,
                UserName = x.UserName,
                Action = x.Action,
                EntityName = x.EntityName,
                EntityId = x.EntityId,
                CreatedAt = x.CreatedAt,
                Changes = parsedChanges
            };
        }).ToList();

        return Result<List<AuditLogDto>>.Ok(result);
    }
}
