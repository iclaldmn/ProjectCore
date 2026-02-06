using Application.Commands;
using Application.Queries.GetProjeList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles = "Admin")]
public class ProjeController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjeCommand command)
    {
       
        var id = await mediator.Send(command);

        return Ok(new
        {
            Id = id,
            Message = "Proje başarıyla oluşturuldu"
        });
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] UpdateProjeCommand command)
    {
        command.Id = id;

        var updatedId = await mediator.Send(command);

        return Ok(new
        {
            Id = updatedId,
            Message = "Proje başarıyla güncellendi"
        });
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetList()
    {
        var result = await mediator.Send(new GetProjeListQuery());
        return Ok(result);
    }
}
