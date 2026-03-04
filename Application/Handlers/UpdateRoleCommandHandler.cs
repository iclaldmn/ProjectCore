using Application.Commands;
using Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers;

public class UpdateRoleCommandHandler(
    RoleManager<IdentityRole<long>> roleManager)
    : IRequestHandler<UpdateRoleCommand, Result<long>>
{
    public async Task<Result<long>> Handle(
        UpdateRoleCommand request,
        CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByIdAsync(request.Id.ToString());

        if (role == null)
            return Result<long>.Fail("Rol bulunamadı.");

        role.Name = request.Name;

        var result = await roleManager.UpdateAsync(role);

        if (!result.Succeeded)
            return Result<long>.Fail("Rol güncellenemedi.");

        return Result<long>.Ok(role.Id, "Rol güncellendi.");
    }
}
