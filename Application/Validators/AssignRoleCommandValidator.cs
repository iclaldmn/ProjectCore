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

public class AssignRoleCommandValidator
    : AbstractValidator<AssignRoleCommand>
{
    public AssignRoleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("Geçersiz kullanıcı Id.");

        RuleFor(x => x.Roles)
            .NotEmpty()
            .WithMessage("En az bir rol seçilmelidir.");
    }
}