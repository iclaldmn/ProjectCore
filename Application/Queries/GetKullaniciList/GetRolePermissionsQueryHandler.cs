using Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetKullaniciList;

public class GetRolePermissionsQueryHandler(
    RoleManager<IdentityRole<long>> roleManager)
    : IRequestHandler<GetRolePermissionsQuery, Result<List<string>>>
{
    public async Task<Result<List<string>>> Handle(
        GetRolePermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByNameAsync(request.RoleName);

        if (role == null)
            return Result<List<string>>.Fail("Rol bulunamadı.");

        var claims = await roleManager.GetClaimsAsync(role);

        var permissions = claims
            .Where(c => c.Type == "permission")
            .Select(c => c.Value)
            .ToList();

        return Result<List<string>>.Ok(permissions);
    }
}