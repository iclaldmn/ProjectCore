using Application.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators;

public class UpdateProjeIlceDagilimiCommandValidator
    : AbstractValidator<UpdateProjeIlceDagilimiCommand>
{
    public UpdateProjeIlceDagilimiCommandValidator()
    {
        // 🔹 Id zorunlu
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("İlçe dağılım Id geçersiz.");

        RuleFor(x => x.IlceId)
            .GreaterThan(0).WithMessage("İlçe seçilmelidir.");

        RuleFor(x => x.IlceyeOdenenBedeli)
            .GreaterThan(0).WithMessage("İlçeye ödenen bedel 0'dan büyük olmalıdır.");


    }
}

