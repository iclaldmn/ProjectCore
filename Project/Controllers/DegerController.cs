using Application.Queries.GetKategoriDegerList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Commands;

namespace API.Controllers;

[ApiController]
[Route("api/degerler")]
//[Authorize(Roles = "Admin")]
public class DegerController(IMediator mediator) : ControllerBase
{
    // 🔹 GET api/degerler/kategori/5
    [HttpGet("kategori/{kategoriId:long}")]
    public async Task<IActionResult> GetByKategori(long kategoriId)
    {
        var result = await mediator.Send(
            new GetDegerByKategoriQuery(kategoriId)
        );

        return Ok(result);
    }

    // 🔹 POST api/degerler
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDegerCommand command)
    {
        var result = await mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    // 🔹 PUT api/degerler/5
    [HttpPut("{id:long}")]
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