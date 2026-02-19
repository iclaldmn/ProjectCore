using Application.Queries.GetProjeList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/deger")]
//[Authorize(Roles = "Admin")]
public class DegerController(IMediator mediator) : ControllerBase
{
    [HttpGet("list/{kategoriId:long}")]
    public async Task<IActionResult> GetByKategori(long kategoriId)
    {
        var result = await mediator.Send(
            new GetDegerByKategoriQuery(kategoriId)
        );

        return Ok(result);
    }
}
