using Application.Commands;
using FluentValidation;

namespace Application.Validators;
public class CreateProjeIlceDagilimiCommandValidator : AbstractValidator<CreateProjeIlceDagilimiCommand>
{
    public CreateProjeIlceDagilimiCommandValidator()
    {
        RuleFor(x => x.IlceId)
            .GreaterThan(0).WithMessage("İlçe seçilmelidir.");

        RuleFor(x => x.IlceyeOdenenBedeli)
            .GreaterThan(0).WithMessage("İlçeye ödenen bedel 0'dan büyük olmalıdır.");
    }
}
