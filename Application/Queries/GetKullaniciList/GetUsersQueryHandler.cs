using Application.DTOs.KullaniciDto;
using Application.Helpers;
using Domain.Entities.Kullanici;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetKullaniciList;

public class GetUsersQueryHandler(
    UserManager<AppUser> userManager)
    : IRequestHandler<GetUsersQuery, Result<List<UserListDto>>>
{
    public async Task<Result<List<UserListDto>>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var users = userManager.Users.ToList();

        var list = new List<UserListDto>();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);

            list.Add(new UserListDto
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                IsActive = user.IsActive,
                Roles = roles.ToList()
            });
        }

        return Result<List<UserListDto>>.Ok(list);
    }
}
