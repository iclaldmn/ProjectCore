using Application.Commands;
using Domain.Entities.Ortak;
using Domain.Entities.ProjeModul;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace Application.Validators;
public class CreateProjeCommandValidator
    : AbstractValidator<CreateProjeCommand>
{
    private readonly IUnitOfWork _uow;

    public CreateProjeCommandValidator(IUnitOfWork uow)
    {
        _uow = uow;

        // 🔹 Proje adı
        RuleFor(x => x.Adi)
            .NotEmpty().WithMessage("Proje adı boş olamaz.")
            .MaximumLength(200).WithMessage("Proje adı 200 karakterden uzun olamaz.");

        // 🔹 Açıklama
        RuleFor(x => x.Aciklama)
            .MaximumLength(500)
            .WithMessage("Açıklama 500 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrWhiteSpace(x.Aciklama));

        // 🔹 Bedeller
        RuleFor(x => x.Bedeli)
            .GreaterThan(0)
            .WithMessage("Proje bedeli 0'dan büyük olmalıdır.");

        RuleFor(x => x.IlaveSozlesmeBedeli)
            .GreaterThanOrEqualTo(0)
            .WithMessage("İlave sözleşme bedeli negatif olamaz.");

        // 🔹 Tarihler
        RuleFor(x => x.BaslangicTarihi)
            .LessThanOrEqualTo(x => x.BitisTarihi)
            .WithMessage("Başlangıç tarihi bitiş tarihinden sonra olamaz.");

        RuleFor(x => x.BitisTarihi)
            .GreaterThanOrEqualTo(x => x.BaslangicTarihi)
            .WithMessage("Bitiş tarihi başlangıç tarihinden önce olamaz.");

        // 🔹 İlçe dağılımları zorunlu
        RuleFor(x => x.IlceDagilimlari)
            .NotNull().WithMessage("İlçe dağılımı listesi boş olamaz.")
            .NotEmpty().WithMessage("En az bir ilçe dağılımı belirtilmelidir.");

        // 🔹 Aynı ilçe tekrar edemez
        RuleFor(x => x.IlceDagilimlari)
            .Must(list =>
                list.GroupBy(i => i.IlceId).All(g => g.Count() == 1))
            .WithMessage("Aynı ilçe birden fazla kez eklenemez.");

        // 🔹 Toplam eşitlik kontrolü (ÖNEMLİ)
        RuleFor(x => x)
            .Must(x =>
                x.IlceDagilimlari.Sum(i => i.IlceyeOdenenBedeli)
                == x.Bedeli + x.IlaveSozlesmeBedeli)
            .WithMessage("İlçe dağılım toplamı proje toplam bedeline eşit olmalıdır.");

        // 🔹 İlçe dağılımı alt validator
        RuleForEach(x => x.IlceDagilimlari)
            .SetValidator(new CreateProjeIlceDagilimiCommandValidator());

        // 🔥 DİNAMİK KATEGORİ ZORUNLULUK KONTROLÜ
        RuleFor(x => x)
            .MustAsync(ZorunluKategoriKontrol)
            .WithMessage("Zorunlu kategori alanları doldurulmalıdır.");

        // 🔹 Aynı kategori birden fazla seçilemez
        RuleFor(x => x.KategoriDegerleri)
            .Must(list => list == null ||
                list.GroupBy(k => k.KategoriId).All(g => g.Count() == 1))
            .WithMessage("Aynı kategori birden fazla seçilemez.");

        RuleFor(x => x.KategoriDegerleri)
            .Must(list => list == null ||
                list.All(k => k.KategoriId > 0 && k.DegerId > 0))
            .WithMessage("Kategori ve değer seçimi geçersiz.");

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
        RuleFor(x => x.Adi)
            .MustAsync(async (adi, cancellation) =>
            {
                if (string.IsNullOrWhiteSpace(adi))
                    return true;

                var normalized = adi.Trim().ToLower();

                return !await _uow.Repository<Proje>()
                    .Query()
                    .AnyAsync(x => x.Adi.ToLower() == normalized && !x.Silindi, cancellation);
            })
            .WithMessage("Bu isimde bir proje zaten var.");

        // 🔹 Nakdi Gerçekleşme
        RuleFor(x => x.NakdiGerceklesmeTutari)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Nakdi gerçekleşme tutarı negatif olamaz.");

        // 🔹 Fiziki Gerçekleşme
        RuleFor(x => x.FizikiGerceklesmeOrani)
            .InclusiveBetween(0, 100)
            .WithMessage("Fiziki gerçekleşme oranı 0 ile 100 arasında olmalıdır.");
        RuleFor(x => x)
            .Must(x => x.NakdiGerceklesmeTutari <= (x.Bedeli + x.IlaveSozlesmeBedeli))
            .WithMessage("Nakdi gerçekleşme tutarı toplam proje bedelini geçemez.");
    }

    private async Task<bool> ZorunluKategoriKontrol(
        CreateProjeCommand command,
        CancellationToken cancellationToken)
    {
        // Aktif + projede göster + zorunlu kategoriler
        var zorunluKategoriler = await _uow.Repository<Kategori>()
            .Query()
            .Where(x => !x.Silindi
                        && x.Aktif
                        && x.ProjedeGoster
                        && x.ProjedeZorunlu)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        if (!zorunluKategoriler.Any())
            return true;

        if (command.KategoriDegerleri == null)
            return false;

        var secilenKategoriIdler = command.KategoriDegerleri
            .Select(x => x.KategoriId)
            .ToList();

        return zorunluKategoriler
            .All(z => secilenKategoriIdler.Contains(z));
    }
}



