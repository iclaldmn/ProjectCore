using API;
using Application.Commands;
using Application.DTOs.ProjeDto;
using Application.Queries.GetProjeList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[ApiController]
[Route("api/projeler")]
[Authorize]
public class ProjeController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [HasPermission("Proje.Create")]
    public async Task<IActionResult> Create([FromBody] CreateProjeCommand command)
    {
        var result = await mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPut("{id:long}")]
    [HasPermission("Proje.Update")]
    public async Task<IActionResult> Update(
    long id,
    UpdateProjeCommand command)
    {
        if (id != command.Id)
            return BadRequest("Id eşleşmiyor");

        var result = await mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [HasPermission("Proje.View")]
    public async Task<ActionResult<ProjeUpdateDto>> GetById(long id)
    {
        var result = await mediator.Send(new GetProjeByIdQuery { Id = id });

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("list")]
    [HasPermission("Proje.View")]
    public async Task<IActionResult> GetList()
    {
        var result = await mediator.Send(new GetProjeListQuery());
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    [HasPermission("Proje.Delete")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await mediator.Send(new DeleteProjeCommand { Id = id });

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    
}
