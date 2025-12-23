using Application.Commands;
using FluentValidation;

namespace Application.Validators;
public class CreateProjeCommandValidator
    : AbstractValidator<CreateProjeCommand>
{
    public CreateProjeCommandValidator()
    {
        // 🔹 Proje adı
        RuleFor(x => x.Adi)
            .NotEmpty().WithMessage("Proje adı boş olamaz.")
            .MaximumLength(200).WithMessage("Proje adı 200 karakterden uzun olamaz.");

        // 🔹 Açıklama
        RuleFor(x => x.Aciklama)
            .MaximumLength(500).WithMessage("Açıklama 500 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrWhiteSpace(x.Aciklama));

        // 🔹 Bedeller
        RuleFor(x => x.Bedeli)
            .GreaterThan(0).WithMessage("Proje bedeli 0'dan büyük olmalıdır.");

        RuleFor(x => x.IlaveSozlesmeBedeli)
            .GreaterThanOrEqualTo(0)
            .WithMessage("İlave sözleşme bedeli negatif olamaz.");

        // 🔹 Lookup Id'ler
        RuleFor(x => x.IhaleTuruId)
            .GreaterThan(0).WithMessage("İhale türü seçilmelidir.");

        RuleFor(x => x.HedefKitleId)
            .GreaterThan(0).WithMessage("Hedef kitle seçilmelidir.");

        RuleFor(x => x.ProjeTipiId)
            .GreaterThan(0).WithMessage("Proje tipi seçilmelidir.");

        RuleFor(x => x.ProjeDurumuId)
            .GreaterThan(0).WithMessage("Proje durumu seçilmelidir.");

        // 🔹 Tarihler
        RuleFor(x => x.BaslangicTarihi)
            .LessThan(x => x.BitisTarihi)
            .WithMessage("Başlangıç tarihi bitiş tarihinden önce olmalıdır.");

        // 🔹 İlçe dağılımları (zorunlu)
        RuleFor(x => x.IlceDagilimlari)
            .NotNull().WithMessage("İlçe dağılımı listesi boş olamaz.")
            .NotEmpty().WithMessage("En az bir ilçe dağılımı belirtilmelidir.");

        // Aynı ilçe birden fazla olamaz
        RuleFor(x => x.IlceDagilimlari)
            .Must(list =>
                list.GroupBy(i => i.IlceId).All(g => g.Count() == 1))
            .WithMessage("Aynı ilçe birden fazla kez eklenemez.");

        // Toplam bedel kontrolü
        RuleFor(x => x)
            .Must(x =>
                x.IlceDagilimlari.Sum(i => i.IlceyeOdenenBedeli)
                <= x.Bedeli + x.IlaveSozlesmeBedeli)
            .WithMessage("İlçe dağılım toplamı proje toplam bedelini aşamaz.");

        // 🔹 İlçe dağılımı validator (SADECE CREATE)
        RuleForEach(x => x.IlceDagilimlari)
            .SetValidator(new CreateProjeIlceDagilimiCommandValidator());
    }
}



