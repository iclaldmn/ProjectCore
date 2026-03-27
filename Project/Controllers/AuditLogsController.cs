using Application.Common;
using Application.Queries.GetLogList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/audit-logs")]
[ApiController]
[Authorize]
public class AuditLogsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [HasPermission("AuditLog.View")]
    public async Task<IActionResult> GetList(
        [FromQuery] string? userName,
        [FromQuery] string? action,
        [FromQuery] int take = 50)
    {
        var result = await mediator.Send(new GetAuditLogsQuery
        {
            UserName = userName,
            Action = action,
            Take = take
        });

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}