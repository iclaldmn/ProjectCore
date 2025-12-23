using Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ProjeController(IMediator mediator) : ControllerBase
{
    [Authorize]
    // ✅ CREATE
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

    // ✅ UPDATE
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] UpdateProjeCommand command)
    {
        // URL'den gelen Id'yi command'a set et
        command.Id = id;

        var updatedId = await mediator.Send(command);

        return Ok(new
        {
            Id = updatedId,
            Message = "Proje başarıyla güncellendi"
        });
    }
}
