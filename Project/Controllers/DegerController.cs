using Application.Queries.GetKategoriDegerList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Commands;

namespace API.Controllers;

[ApiController]
[Route("api/degerler")]
[Authorize]
public class DegerController(IMediator mediator) : ControllerBase
{
    // 🔹 GET api/degerler/kategori/5
    [HttpGet("kategori/{kategoriId:long}")]
    [HasPermission("Deger.View")]
    public async Task<IActionResult> GetByKategori(long kategoriId)
    {
        var result = await mediator.Send(
            new GetDegerByKategoriQuery(kategoriId)
        );

        return Ok(result);
    }

    // 🔹 POST api/degerler
    [HttpPost]
    [HasPermission("Deger.Create")]
    public async Task<IActionResult> Create([FromBody] CreateDegerCommand command)
    {
        var result = await mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    // 🔹 PUT api/degerler/5
    [HttpPut("{id:long}")]
    [HasPermission("Deger.Update")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateDegerCommand command)
    {
        command.Id = id;

        var result = await mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    // 🔹 DELETE api/degerler/5
    [HttpDelete("{id:long}")]
    [HasPermission("Deger.Delete")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await mediator.Send(
            new DeleteDegerCommand { Id = id }
        );

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}