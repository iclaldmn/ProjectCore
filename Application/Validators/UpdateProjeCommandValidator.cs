using Application.Commands;
using Domain.Entities.Ortak;
using Domain.Entities.ProjeModul;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
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

        // 🔹 Kategori değerleri kontrolü
        RuleFor(x => x.KategoriDegerleri)
            .Must(list => list == null || list.All(k => k.KategoriId > 0 && k.DegerId > 0))
            .WithMessage("Kategori ve değer seçimi geçersiz.");

        // Aynı kategori birden fazla seçilemez
        RuleFor(x => x.KategoriDegerleri)
            .Must(list => list == null ||
                list.GroupBy(k => k.KategoriId).All(g => g.Count() == 1))
            .WithMessage("Aynı kategori birden fazla seçilemez.");

        RuleFor(x => x)
        .MustAsync(async (command, cancellation) =>
        {
            var zorunluKategoriler = await _uow.Repository<Kategori>()
                .Query()
                .Where(k => k.ProjedeZorunlu)
                .Select(k => k.Id)
                .ToListAsync(cancellation);

            if (zorunluKategoriler.Count == 0)
                return true;

            if (command.KategoriDegerleri == null)
                return false;

            var secilenKategoriIds = command.KategoriDegerleri
                .Select(k => k.KategoriId)
                .ToList();

            return zorunluKategoriler.All(z => secilenKategoriIds.Contains(z));
        })
        .WithMessage("Zorunlu kategoriler için değer seçilmelidir.");

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
             .When(x => x.IlceDagilimlari != null && x.IlceDagilimlari.Any(i => i.Id > 0));

        RuleForEach(x => x.KategoriDegerleri)
            .MustAsync(async (item, cancellation) =>
            {
                return await _uow.Repository<Deger>()
                    .AnyAsync(d =>
                        d.Id == item.DegerId &&
                        d.KategoriId == item.KategoriId &&
                        !d.Silindi,
                        cancellation);
            })
            .WithMessage("Seçilen değer ilgili kategoriye ait değil.");

        RuleFor(x => x)
            .Must(x =>
                x.IlceDagilimlari == null ||
                x.IlceDagilimlari.Sum(i => i.IlceyeOdenenBedeli)
                == (x.Bedeli + x.IlaveSozlesmeBedeli)
            )
            .WithMessage("İlçe dağılım toplamı proje toplam bedeline eşit olmalıdır.");

    }

    private async Task<bool> ProjeVarMi(
        long projeId,
        CancellationToken cancellationToken)
    {
        return await _uow.Repository<Proje>()
            .AnyAsync(x => x.Id == projeId, cancellationToken);
    }
}


