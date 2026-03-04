using Application.Commands;
using FluentValidation;

namespace Application.Validators;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("Kullanıcı adı zorunludur.")
            .MaximumLength(100)
            .WithMessage("Kullanıcı adı 100 karakterden uzun olamaz.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Şifre zorunludur.")
            .MinimumLength(3)
            .WithMessage("Şifre en az 3 karakter olmalıdır.");
    }
}