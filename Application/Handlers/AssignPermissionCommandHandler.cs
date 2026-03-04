using Application.Commands;
using Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers;
public class AssignPermissionCommandHandler(
    RoleManager<IdentityRole<long>> roleManager)
    : IRequestHandler<AssignPermissionCommand, Result<string>>
{
    public async Task<Result<string>> Handle(
        AssignPermissionCommand request,
        CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByNameAsync(request.RoleName);

        if (role == null)
            return Result<string>.Fail("Rol bulunamadı.");

        var existingClaims = await roleManager.GetClaimsAsync(role);

        // Mevcut permission claimleri sil
        foreach (var claim in existingClaims
                     .Where(c => c.Type == "permission"))
        {
            await roleManager.RemoveClaimAsync(role, claim);
        }

        // Yeni permissionları ekle
        foreach (var permission in request.Permissions)
        {
            await roleManager.AddClaimAsync(
                role,
                new Claim("permission", permission));
        }

        return Result<string>.Ok("Güncellendi.");
    }
}