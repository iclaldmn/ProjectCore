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

public class CreateUserCommandValidator
    : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<long>> roleManager)
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Kullanıcı adı zorunludur.")
            .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır.");

        RuleFor(x => x)
            .MustAsync(async (command, cancellation) =>
            {
                var existingUser = await userManager.FindByNameAsync(command.UserName);
                return existingUser == null;
            })
            .WithMessage("Bu kullanıcı adı zaten kullanılıyor.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email zorunludur.")
            .EmailAddress().WithMessage("Geçerli bir email giriniz.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre zorunludur.")
            .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.")
            .Matches("[0-9]").WithMessage("Şifre en az bir rakam içermelidir.");

        RuleFor(x => x)
            .MustAsync(async (command, cancellation) =>
            {
                var existingUser = await userManager.FindByEmailAsync(command.Email);
                return existingUser == null;
            })
            .WithMessage("Bu email zaten kayıtlı.");

        RuleForEach(x => x.Roles)
            .MustAsync(async (role, cancellation) =>
            {
                return await roleManager.RoleExistsAsync(role);
            })
            .WithMessage("Geçersiz rol seçildi.");
    }
}