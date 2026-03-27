using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public interface IAuditLogService
{
    Task LogCreateAsync(
            string entityName, 
            string entityId, 
            object newValues);
    Task LogUpdateAsync(
            string entityName,
            string entityId,
            object changes);
    Task LogDeleteAsync(
            string entityName,
            string entityId,
            object oldValues);
}