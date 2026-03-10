using Application.Commands;
using Domain.Entities.Kullanici;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators;

public class UpdateUserCommandValidator
    : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Geçersiz kullanıcı Id.");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Kullanıcı adı boş olamaz.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email zorunludur.")
            .EmailAddress().WithMessage("Geçerli email giriniz.");

        RuleFor(x => x)
          .MustAsync(async (command, cancellation) =>
          {
              var user = await userManager.FindByNameAsync(command.UserName);
              return user == null || user.Id == command.Id;
          })
          .WithMessage("Bu kullanıcı adı başka bir kullanıcıya ait.");

        RuleForEach(x => x.Roles)
            .MustAsync(async (role, cancellation) =>
            {
                return await roleManager.RoleExistsAsync(role);
            })
            .WithMessage("Geçersiz rol seçildi.");
    }
}