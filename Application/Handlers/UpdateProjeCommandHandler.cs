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

namespace Application.Handlers;

public class UpdateProjeCommandHandler(
    IUnitOfWork uow,
    IMapper mapper
) : IRequestHandler<UpdateProjeCommand, Result<long>>
{
    //public async Task<Result<long>> Handle(
    // UpdateProjeCommand request,
    // CancellationToken cancellationToken)
    //{
    //    var entity = await uow.Repository<Proje>()
    //        .GetWithIncludeAsync(
    //            x => x.Id == request.Id,
    //            cancellationToken,
    //            x => x.IlceDagilimlari
    //        );

    //    if (entity == null)
    //        return Result<long>.Fail("Proje bulunamadı");

    //    // Scalar alanlar
    //    entity.Adi = request.Adi;
    //    entity.Aciklama = request.Aciklama;
    //    entity.Bedeli = request.Bedeli;
    //    entity.IlaveSozlesmeBedeli = request.IlaveSozlesmeBedeli;
    //    entity.IhaleTuruId = request.IhaleTuruId;
    //    entity.HedefKitleId = request.HedefKitleId;
    //    entity.ProjeTipiId = request.ProjeTipiId;
    //    entity.ProjeDurumuId = request.ProjeDurumuId;
    //    entity.BaslangicTarihi = request.BaslangicTarihi;
    //    entity.BitisTarihi = request.BitisTarihi;

    //    var ilceList = request.IlceDagilimlari
    //        ?? new List<UpdateProjeIlceDagilimiCommand>();

    //    var mevcutDbIlceList = entity.IlceDagilimlari.ToList();

    //    // 1️⃣ DELETE
    //    foreach (var mevcut in mevcutDbIlceList)
    //    {
    //        if (!ilceList.Any(x => x.Id.HasValue && x.Id == mevcut.Id))
    //        {
    //            entity.IlceDagilimlari.Remove(mevcut);
    //        }
    //    }

    //    // 2️⃣ UPDATE
    //    foreach (var mevcut in mevcutDbIlceList)
    //    {
    //        var requestItem = ilceList
    //            .FirstOrDefault(x => x.Id == mevcut.Id);

    //        if (requestItem != null)
    //        {
    //            mevcut.IlceyeOdenenBedeli = requestItem.IlceyeOdenenBedeli;
    //        }
    //    }

    //    // 3️⃣ INSERT
    //    foreach (var item in ilceList.Where(x => !x.Id.HasValue))
    //    {
    //        entity.IlceDagilimlari.Add(new ProjeIlceDagilimi
    //        {
    //            IlceId = item.IlceId,
    //            IlceyeOdenenBedeli = item.IlceyeOdenenBedeli
    //        });
    //    }

    //    entity.ToplamBedel = entity.Bedeli + entity.IlaveSozlesmeBedeli;

    //    var dagilimToplam = entity.IlceDagilimlari
    //        .Sum(x => x.IlceyeOdenenBedeli);

    //    if (dagilimToplam != entity.ToplamBedel)
    //        return Result<long>.Fail(
    //            "İlçe dağılım toplamı proje toplamına eşit olmalıdır"
    //        );

    //    await uow.SaveAsync(cancellationToken);

    //    return Result<long>.Ok(entity.Id, "Proje başarıyla güncellendi");
    //}

    public async Task<Result<long>> Handle(
    UpdateProjeCommand request,
    CancellationToken cancellationToken)
    {
        var entity = await uow.Repository<Proje>()
            .Query()
            .Include(x => x.IlceDagilimlari)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null)
            return Result<long>.Fail("Proje bulunamadı");

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

        // Toplam hesap
        entity.ToplamBedel = entity.Bedeli + entity.IlaveSozlesmeBedeli;

        var dagilimToplam = entity.IlceDagilimlari
            .Where(x => !x.Silindi)
            .Sum(x => x.IlceyeOdenenBedeli);

        if (dagilimToplam != entity.ToplamBedel)
            return Result<long>.Fail(
                "İlçe dağılım toplamı proje toplamına eşit olmalıdır");

        await uow.SaveAsync(cancellationToken);

        return Result<long>.Ok(entity.Id, "Proje başarıyla güncellendi");
    }
}



