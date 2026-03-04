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
public class CreateUserCommandHandler(
    UserManager<AppUser> userManager,
    RoleManager<IdentityRole<long>> roleManager)
    : IRequestHandler<CreateUserCommand, Result<long>>
{
    public async Task<Result<long>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = new AppUser
        {
            UserName = request.UserName,
            Email = request.Email,
            IsActive = true
        };

        var createResult =
            await userManager.CreateAsync(user, request.Password);

        if (!createResult.Succeeded)
        {
            var errors = string.Join(" | ",
                createResult.Errors.Select(e => e.Description));

            return Result<long>.Fail(errors);
        }

        // 🔹 Role kontrolü
        if (request.Roles != null && request.Roles.Any())
        {
            foreach (var role in request.Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    return Result<long>.Fail($"Rol bulunamadı: {role}");
            }

            var roleResult =
                await userManager.AddToRolesAsync(user, request.Roles);

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(" | ",
                    roleResult.Errors.Select(e => e.Description));

                return Result<long>.Fail(errors);
            }
        }

        return Result<long>.Ok(user.Id, "Kullanıcı oluşturuldu.");
    }
}