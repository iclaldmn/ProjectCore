using Application.Commands;
using Application.Helpers;
using Application.Services;
using AutoMapper;
using Domain.Entities.Log;
using Domain.Entities.Ortak;
using Domain.Entities.ProjeModul;
using MediatR;
using Repository.Interfaces;

namespace Application.Handlers;

public class CreateProjeCommandHandler(
    IUnitOfWork uow,
    IMapper mapper,
    IAuditLogService auditLog
) : IRequestHandler<CreateProjeCommand, Result<long>>
{
    public async Task<Result<long>> Handle(
        CreateProjeCommand request,
        CancellationToken cancellationToken)
    {
        // 🔹 İlçe kontrolü
        if (request.IlceDagilimlari == null || !request.IlceDagilimlari.Any())
            return Result<long>.Fail("En az bir ilçe eklenmelidir");

        var duplicateIlce = request.IlceDagilimlari
            .GroupBy(x => x.IlceId)
            .Any(g => g.Count() > 1);

        if (duplicateIlce)
            return Result<long>.Fail("Aynı ilçe birden fazla eklenemez");

        // 🔥 Dinamik kategori kontrolü
        if (request.KategoriDegerleri != null && request.KategoriDegerleri.Any())
        {
            var duplicateKategori = request.KategoriDegerleri
                .GroupBy(x => x.KategoriId)
                .Any(g => g.Count() > 1);

            if (duplicateKategori)
                return Result<long>.Fail("Aynı kategori birden fazla seçilemez");
        }

        var entity = mapper.Map<Proje>(request);

        // 🔥 Toplam hesap
        entity.ToplamBedel = entity.Bedeli + entity.IlaveSozlesmeBedeli;

        var dagilimToplam = request.IlceDagilimlari
            .Sum(x => x.IlceyeOdenenBedeli);

        if (dagilimToplam != entity.ToplamBedel)
            return Result<long>.Fail(
                "İlçe dağılım toplamı proje toplamına eşit olmalıdır");

        // 🔥 İlçe ekleme
        entity.IlceDagilimlari = request.IlceDagilimlari
            .Select(x => new ProjeIlceDagilimi
            {
                IlceId = x.IlceId,
                IlceyeOdenenBedeli = x.IlceyeOdenenBedeli
            }).ToList();

        // 🔥 Dinamik kategori ekleme
        entity.KategoriDegerleri = request.KategoriDegerleri?
            .Select(x => new ProjeKategoriDeger
            {
                KategoriId = x.KategoriId,
                DegerId = x.DegerId
            }).ToList() ?? new List<ProjeKategoriDeger>();

        await uow.Repository<Proje>().AddAsync(entity);
        await uow.SaveAsync(cancellationToken);

        // 🔥 Audit Log
        await auditLog.LogCreateAsync("Proje", entity.Id.ToString());
        //await auditLog.LogAsync(
        //    "Create",
        //    "Proje",
        //    entity.Id.ToString());

        return Result<long>.Ok(entity.Id, "Proje başarıyla oluşturuldu");
    }
}
