using Application.Commands;
using Application.Helpers;
using Domain.Entities.Kullanici;
using MediatR;
using Microsoft.AspNetCore.Identity;

public class AssignRoleCommandHandler(
    UserManager<AppUser> userManager,
    RoleManager<IdentityRole<long>> roleManager)
    : IRequestHandler<AssignRoleCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        AssignRoleCommand request,
        CancellationToken cancellationToken)
    {
        // 🔹 Kullanıcı kontrolü
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null)
            return Result<bool>.Fail("Kullanıcı bulunamadı.");

        // 🔹 Roller gerçekten var mı kontrolü
        foreach (var role in request.Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                return Result<bool>.Fail($"Rol bulunamadı: {role}");
        }

        // 🔹 Mevcut roller
        var existingRoles = await userManager.GetRolesAsync(user);

        // 🔹 Eski rolleri kaldır
        var removeResult = await userManager.RemoveFromRolesAsync(user, existingRoles);
        if (!removeResult.Succeeded)
        {
            return Result<bool>.Fail(
                string.Join(", ", removeResult.Errors.Select(e => e.Description))
            );
        }

        // 🔹 Yeni rolleri ekle
        var addResult = await userManager.AddToRolesAsync(user, request.Roles);
        if (!addResult.Succeeded)
        {
            return Result<bool>.Fail(
                string.Join(", ", addResult.Errors.Select(e => e.Description))
            );
        }

        return Result<bool>.Ok(true, "Roller başarıyla güncellendi.");
    }
}