using Application.Commands;
using Domain.Entities.ProjeModul;
using FluentValidation;
using Repository.Interfaces;

namespace Application.Validators;

public class UpdateProjeCommandValidator
    : AbstractValidator<UpdateProjeCommand>
{
    private readonly IUnitOfWork _uow;

    public UpdateProjeCommandValidator(IUnitOfWork uow)
    {
        _uow = uow;

        // 🔹 Id zorunlu + var mı?
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Proje Id geçersiz.")
            .MustAsync(ProjeVarMi)
            .WithMessage("Güncellenecek proje bulunamadı.");

        // 🔹 Proje adı (opsiyonel ama doluysa geçerli olmalı)
        RuleFor(x => x.Adi)
            .NotEmpty().WithMessage("Proje adı boş olamaz.")
            .MaximumLength(200).WithMessage("Proje adı 200 karakterden uzun olamaz.")
            .When(x => x.Adi != null);

        // 🔹 Açıklama
        RuleFor(x => x.Aciklama)
            .MaximumLength(500).WithMessage("Açıklama 500 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrWhiteSpace(x.Aciklama));

        // 🔹 Bedeller (gönderildiyse kontrol edilir)
        RuleFor(x => x.Bedeli)
            .GreaterThan(0)
            .When(x => x.Bedeli != default)
            .WithMessage("Proje bedeli 0'dan büyük olmalıdır.");

        RuleFor(x => x.IlaveSozlesmeBedeli)
            .GreaterThanOrEqualTo(0)
            .When(x => x.IlaveSozlesmeBedeli != default)
            .WithMessage("İlave sözleşme bedeli negatif olamaz.");

        // 🔹 Lookup Id'ler (opsiyonel)
        RuleFor(x => x.IhaleTuruId)
            .GreaterThan(0)
            .When(x => x.IhaleTuruId != default)
            .WithMessage("İhale türü seçilmelidir.");

        RuleFor(x => x.HedefKitleId)
            .GreaterThan(0)
            .When(x => x.HedefKitleId != default)
            .WithMessage("Hedef kitle seçilmelidir.");

        RuleFor(x => x.ProjeTipiId)
            .GreaterThan(0)
            .When(x => x.ProjeTipiId != default)
            .WithMessage("Proje tipi seçilmelidir.");

        RuleFor(x => x.ProjeDurumuId)
            .GreaterThan(0)
            .When(x => x.ProjeDurumuId != default)
            .WithMessage("Proje durumu seçilmelidir.");

        // 🔹 Tarihler (ikisi de gönderildiyse kontrol)
        RuleFor(x => x)
            .Must(x =>
                x.BaslangicTarihi == default ||
                x.BitisTarihi == default ||
                x.BaslangicTarihi < x.BitisTarihi
            )
            .WithMessage("Başlangıç tarihi bitiş tarihinden önce olmalıdır.");

        // 🔹 İlçe dağılımları (opsiyonel)
        RuleFor(x => x.IlceDagilimlari)
            .NotEmpty()
            .When(x => x.IlceDagilimlari != null)
            .WithMessage("En az bir ilçe dağılımı belirtilmelidir.");

        // Aynı ilçe birden fazla olamaz
        RuleFor(x => x.IlceDagilimlari)
            .Must(list => list == null ||
                list.GroupBy(i => i.IlceId).All(g => g.Count() == 1))
            .WithMessage("Aynı ilçe birden fazla kez eklenemez.");

        // Toplam bedel kontrolü (ilçe dağılımı gönderildiyse)
        RuleFor(x => x)
            .Must(x =>
                x.IlceDagilimlari == null ||
                x.IlceDagilimlari.Sum(i => i.IlceyeOdenenBedeli)
                <= (x.Bedeli != default
                    ? x.Bedeli + x.IlaveSozlesmeBedeli
                    : decimal.MaxValue)
            )
            .WithMessage("İlçe dağılım toplamı proje toplam bedelini aşamaz.");

        // 🔹 İlçe validator (create / update ayrımı)
        RuleForEach(x => x.IlceDagilimlari)
             .SetValidator(new UpdateProjeIlceDagilimiCommandValidator())
             .When(x => x.IlceDagilimlari.Any(i => i.Id > 0));

    }

    private async Task<bool> ProjeVarMi(
        long projeId,
        CancellationToken cancellationToken)
    {
        return await _uow.Repository<Proje>()
            .AnyAsync(x => x.Id == projeId, cancellationToken);
    }
}


