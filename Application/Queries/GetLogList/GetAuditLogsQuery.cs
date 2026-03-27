using Application.DTOs.LogDto;
using Application.Helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetLogList;

public class GetAuditLogsQuery : IRequest<Result<List<AuditLogDto>>>
{
    public int Take { get; set; } = 50;

    // 🔥 filtreler (opsiyonel ama çok faydalı)
    public string? UserName { get; set; }
    public string? Action { get; set; }
}