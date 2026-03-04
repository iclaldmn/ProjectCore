using Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // 🔐 LOGIN
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return Unauthorized(result);

        return Ok(result);
    }

    // 🔓 LOGOUT
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        // JWT stateless olduğu için server tarafında işlem yok
        return Ok(new
        {
            Success = true,
            Message = "Çıkış başarılı"
        });
    }
}