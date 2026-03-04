using Application.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators;

public class AssignPermissionCommandValidator
    : AbstractValidator<AssignPermissionCommand>
{
    public AssignPermissionCommandValidator()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty();

        RuleFor(x => x.Permissions)
            .NotNull();
    }
}
