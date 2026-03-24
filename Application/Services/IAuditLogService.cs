using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public interface IAuditLogService
{
    Task LogAsync(string action, string entityName, string entityId);
    Task LogCreateAsync(string entityName, string entityId);
}