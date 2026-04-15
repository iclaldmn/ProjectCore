using Application.Commands;
using Application.Validators;
using AutoMapper;
using Domain.Entities.ProjeModul;
using MediatR;
using Repository.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Application.Helpers;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Ortak;

namespace Application.Handlers;

public class UpdateProjeCommandHandler(
    IUnitOfWork uow,
    IMapper mapper
) : IRequestHandler<UpdateProjeCommand, Result<long>>
{
    public async Task<Result<long>> Handle(
    UpdateProjeCommand request,
    CancellationToken cancellationToken)
    {
        var entity = await uow.Repository<Proje>()
            .Query()
            .Include(x => x.IlceDagilimlari)
            .Include(x => x.KategoriDegerleri)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null)
            return Result<long>.Fail("Proje bulunamadı");

        var exists = await uow.Repository<Proje>()
            .Query()
            .AnyAsync(x => x.Adi == request.Adi
                        && x.Id != request.Id
                        && !x.Silindi,
                        cancellationToken);

        if (exists)
            return Result<long>.Fail("Bu isimde bir proje zaten var");


        // ✅ Scalar alanları otomatik map et
        mapper.Map(request, entity);

        var requestList = request.IlceDagilimlari
     ?? new List<UpdateProjeIlceDagilimiCommand>();

        var mevcutDbIlceList = entity.IlceDagilimlari.ToList();


        // 1️⃣ DELETE
        foreach (var dbItem in mevcutDbIlceList)
        {
            var existsInRequest = requestList
                .Any(req => req.Id.HasValue && req.Id == dbItem.Id);

            if (!existsInRequest)
            {
                entity.IlceDagilimlari.Remove(dbItem);
            }
        }


        // 2️⃣ UPDATE
        foreach (var dbItem in entity.IlceDagilimlari)
        {
            var reqItem = requestList
                .FirstOrDefault(x => x.Id == dbItem.Id);

            if (reqItem != null)
            {
                dbItem.IlceyeOdenenBedeli = reqItem.IlceyeOdenenBedeli;
            }
        }


        // 3️⃣ INSERT
        foreach (var reqItem in requestList.Where(x => !x.Id.HasValue))
        {
            entity.IlceDagilimlari.Add(new ProjeIlceDagilimi
            {
                IlceId = reqItem.IlceId,
                IlceyeOdenenBedeli = reqItem.IlceyeOdenenBedeli
            });
        }

        // ================= KATEGORİ DEĞERLERİ =================

        var requestKategoriList = request.KategoriDegerleri
            ?? new List<ProjeKategoriDegerCommand>();

        var mevcutDbKategoriList = entity.KategoriDegerleri.ToList();

        // 1️⃣ DELETE
        foreach (var dbItem in mevcutDbKategoriList)
        {
            var existsInRequest = requestKategoriList
                .Any(req =>
                    req.KategoriId == dbItem.KategoriId &&
                    req.DegerId == dbItem.DegerId);

            if (!existsInRequest)
            {
                entity.KategoriDegerleri.Remove(dbItem);
            }
        }

        // 2️⃣ INSERT
        foreach (var reqItem in requestKategoriList)
        {
            var existsInDb = entity.KategoriDegerleri
                .Any(db =>
                    db.KategoriId == reqItem.KategoriId &&
                    db.DegerId == reqItem.DegerId);

            if (!existsInDb)
            {
                entity.KategoriDegerleri.Add(new ProjeKategoriDeger
                {
                    KategoriId = reqItem.KategoriId,
                    DegerId = reqItem.DegerId
                });
            }
        }

        // Toplam hesap
        entity.ToplamBedel = entity.Bedeli + entity.IlaveSozlesmeBedeli;

        var dagilimToplam = entity.IlceDagilimlari
            .Where(x => !x.Silindi)
            .Sum(x => x.IlceyeOdenenBedeli);

        if (Math.Abs(dagilimToplam - entity.ToplamBedel) > 0.01m)
            return Result<long>.Fail(
                "İlçe dağılım toplamı proje toplamına eşit olmalıdır");

        await uow.SaveAsync(cancellationToken);

        return Result<long>.Ok(entity.Id, "Proje başarıyla güncellendi");
    }
}



