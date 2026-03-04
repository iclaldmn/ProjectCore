
using Application.Commands;
using Application.Queries.GetKullaniciList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/kullanicilar")]
[ApiController]
[Authorize]
public class UsersController(IMediator mediator) : ControllerBase
{
    // 🔹 Kullanıcı Liste
    [HttpGet("list")]
    [HasPermission("User.View")]
    public async Task<IActionResult> GetList()
    {
        var result = await mediator.Send(new GetUsersQuery());

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    // 🔹 Kullanıcı Oluştur
    [HttpPost]
    [HasPermission("User.Create")]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
    {
        var result = await mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Created("", result); // 201 döner
    }

    // 🔹 Kullanıcı Güncelle
    [HttpPut("{id}")]
    [HasPermission("User.Update")]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] UpdateUserCommand command)
    {
        if (id != command.Id)
            return BadRequest("Id uyuşmuyor.");

        var result = await mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    // 🔹 Rol Ata
    [HttpPut("assign-role")]
    [HasPermission("User.RoleAssign")]
    public async Task<IActionResult> AssignRole(
        [FromBody] AssignRoleCommand command)
    {
        var result = await mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    // 🔹 Rol Listele
    [HttpGet("roles")]
    [HasPermission("User.View")]
    public async Task<IActionResult> GetRoles()
    {
        var result = await mediator.Send(new GetRolesQuery());

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}