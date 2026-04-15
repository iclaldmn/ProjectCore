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

public class UpdateUserCommandHandler(
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager)
    : IRequestHandler<UpdateUserCommand, Result<long>>
{
    public async Task<Result<long>> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());

        if (user == null)
            return Result<long>.Fail("Kullanıcı bulunamadı.");

        // 🔹 Email başka kullanıcıda var mı?
        var existingEmailUser = await userManager.FindByEmailAsync(request.Email);
        if (existingEmailUser != null && existingEmailUser.Id != user.Id)
            return Result<long>.Fail("Bu email başka kullanıcı tarafından kullanılıyor.");//bunu validation kısmında çözebilirsin burada çözemediklerini yaz

        // 🔹 Basic bilgiler
        user.UserName = request.UserName;
        user.Email = request.Email;
        user.IsActive = request.IsActive;

        var updateResult = await userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            var errors = string.Join(" | ",
                updateResult.Errors.Select(e => e.Description));

            return Result<long>.Fail(errors);
        }

        // 🔹 ROLE GÜNCELLEME (EN KRİTİK KISIM)
        var existingRoles = await userManager.GetRolesAsync(user);

        if (existingRoles.Any())
        {
            var removeResult =
                await userManager.RemoveFromRolesAsync(user, existingRoles);

            if (!removeResult.Succeeded)
                return Result<long>.Fail("Rol kaldırma hatası.");
        }

        if (request.Roles != null && request.Roles.Any())
        {
            foreach (var role in request.Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    return Result<long>.Fail($"Rol bulunamadı: {role}");
            }

            var addResult =
                await userManager.AddToRolesAsync(user, request.Roles);

            if (!addResult.Succeeded)
            {
                var errors = string.Join(" | ",
                    addResult.Errors.Select(e => e.Description));

                return Result<long>.Fail(errors);
            }
        }

        return Result<long>.Ok(user.Id, "Kullanıcı güncellendi.");
    }
}