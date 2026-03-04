using Application.Commands;
using Application.Queries.GetKategoriDegerList;
using Application.Queries.GetProjeList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/kategoriler")]
[Authorize]
public class KategoriController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [HasPermission("Kategori.View")]
    public async Task<IActionResult> Get()
    {
        var result = await mediator.Send(new GetKategorilerQuery());
        return Ok(result);
    }

    [HttpPost]
    [HasPermission("Kategori.Create")]
    public async Task<IActionResult> Create(CreateKategoriCommand command)
    {
        var result = await mediator.Send(command);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [HasPermission("Kategori.Update")]
    public async Task<IActionResult> Update(long id, UpdateKategoriCommand command)
    {
        command.Id = id;
        var result = await mediator.Send(command);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [HasPermission("Kategori.Delete")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await mediator.Send(new DeleteKategoriCommand { Id = id });
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpGet("proje")]
    [HasPermission("Kategori.View")]
    public async Task<IActionResult> GetProjeKategorileri()
    {
        var result = await mediator.Send(new GetProjeKategorileriQuery());

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}