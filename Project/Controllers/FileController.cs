using Application.Commands;
using Application.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/files")]
public class FileController(IMediator mediator) : ControllerBase
{

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] UploadFileCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    //Tekli dosya silme işlemi
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await mediator.Send(new DeleteFileCommand
        {
            FileReferenceId = id
        });

        return Ok(result);
    }

    //Toplu dosya işlemleri
    public async Task<IActionResult> RemoveByEntity(long id)
    {
        var result = await mediator.Send(new DeleteFilesByEntityCommand
        {
            EntityId = id,
            EntityName = "Project" // şimdilik sabit
        });

        return Ok(result);
    }
}