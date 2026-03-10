using Application.Commands;
using Application.Helpers;
using Domain.Entities.Kullanici;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers;

public class CreateRoleCommandHandler(
    RoleManager<AppRole> roleManager)
    : IRequestHandler<CreateRoleCommand, Result<long>>
{
    public async Task<Result<long>> Handle(
        CreateRoleCommand request,
        CancellationToken cancellationToken)
    {
        if (await roleManager.RoleExistsAsync(request.Name))
            return Result<long>.Fail("Bu rol zaten mevcut.");

        var role = new AppRole
        {
            Name = request.Name
        };

        var result = await roleManager.CreateAsync(role);

        if (!result.Succeeded)
            return Result<long>.Fail("Rol oluşturulamadı.");

        return Result<long>.Ok(role.Id, "Rol başarıyla oluşturuldu.");
    }
}