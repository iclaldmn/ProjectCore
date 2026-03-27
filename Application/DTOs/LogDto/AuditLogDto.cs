using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.LogDto;

public class AuditLogDto
{
    public long Id { get; set; }
    public string UserName { get; set; }
    public string Action { get; set; }
    public string EntityName { get; set; }
    public string EntityId { get; set; }
    public DateTime CreatedAt { get; set; }
    public object? Changes { get; set; }
}
