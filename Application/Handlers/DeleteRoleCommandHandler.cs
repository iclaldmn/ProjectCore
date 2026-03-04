using Application.Commands;
using Application.Helpers;
using Domain.Entities.Kullanici;
using MediatR;
using Microsoft.AspNetCore.Identity;

public class DeleteRoleCommandHandler(
    RoleManager<IdentityRole<long>> roleManager,
    UserManager<AppUser> userManager)
    : IRequestHandler<DeleteRoleCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        DeleteRoleCommand request,
        CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByIdAsync(request.Id.ToString());

        if (role == null)
            return Result<bool>.Fail("Rol bulunamadı.");

        // 🔥 Role’a bağlı kullanıcı var mı kontrolü
        var usersInRole = await userManager.GetUsersInRoleAsync(role.Name!);

        if (usersInRole.Any())
        {
            return Result<bool>.Fail(
                "Bu role bağlı kullanıcılar var. Önce kullanıcıları kaldırın."
            );
        }

        var result = await roleManager.DeleteAsync(role);

        if (!result.Succeeded)
            return Result<bool>.Fail("Rol silinemedi.");

        return Result<bool>.Ok(true, "Rol silindi.");
    }
}