using Application.Commands;
using Application.Queries;
using Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Queries.GetKullaniciList;
using Application.Common;

namespace API.Controllers;

[Route("api/roller")]
[ApiController]
[Authorize] // 🔐 Sadece login zorunlu
public class RolesController(IMediator mediator) : ControllerBase
{
    // 🔹 Rol Listeleme
    [HttpGet]
    [HasPermission("Role.View")]
    public async Task<IActionResult> GetList()
    {
        var result = await mediator.Send(new GetRolesQuery());

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    // 🔹 Rol Oluşturma
    [HttpPost]
    [HasPermission("Role.Create")]
    public async Task<IActionResult> Create(
        [FromBody] CreateRoleCommand command)
    {
        var result = await mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    // 🔹 Rol Güncelleme
    [HttpPut("{id:long}")]
    [HasPermission("Role.Update")]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] UpdateRoleCommand command)
    {
        if (id != command.Id)
            return BadRequest("Id eşleşmiyor.");

        var result = await mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    // 🔹 Rol Silme
    [HttpDelete("{id:long}")]
    [HasPermission("Role.Delete")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await mediator.Send(
            new DeleteRoleCommand { Id = id });

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    // 🔹 Rolün Permissionlarını Getir
    [HttpGet("{roleName}/permissions")]
    [HasPermission("Role.PermissionAssign")]
    public async Task<IActionResult> GetPermissions(string roleName)
    {
        var result = await mediator.Send(
            new GetRolePermissionsQuery { RoleName = roleName });

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    // 🔹 Rolün Permissionlarını Güncelle (Replace Mantığı)
    [HttpPut("assign-permission")]
    [HasPermission("Role.PermissionAssign")]
    public async Task<IActionResult> AssignPermission(
        [FromBody] AssignPermissionCommand command)
    {
        var result = await mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("permissions/grouped")]
    [HasPermission("Role.PermissionAssign")]
    public IActionResult GetGroupedPermissions()
    {
        var permissions = Permissions.GetGrouped();
        return Ok(Result<Dictionary<string, List<string>>>.Ok(permissions));
    }


}

