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
using Application.Services;
using Microsoft.AspNetCore.Http;

namespace Application.Handlers;

public class UpdateProjeCommandHandler(
    IUnitOfWork uow,
    IMapper mapper,
    IAuditLogService auditLog,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<UpdateProjeCommand, Result<long>>
{
    public async Task<Result<long>> Handle(
    UpdateProjeCommand request,
    CancellationToken cancellationToken)
    {
        var daireIdClaim = httpContextAccessor.HttpContext?
            .User
            .FindFirst("DaireBaskanligiId")
            ?.Value;

        if (!long.TryParse(daireIdClaim, out var daireId))
        {
            return Result<long>.Fail(
                "Kullanıcının daire başkanlığı bilgisi bulunamadı.");
        }

        var entity = await uow.Repository<Proje>()
            .Query()
            .Include(x => x.IlceDagilimlari)
                .ThenInclude(x => x.FaaliyetAlanlari)
            .Include(x => x.KategoriDegerleri)
            .Include(x => x.PaydasBirimler)
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

        //===========PAYDAŞ DAİRE BAŞKANLIĞI=============//

        var requestPaydaslar =
        request.PaydasDaireBaskanligiIds?
            .Distinct()
            .ToList()
        ?? [];

        if (requestPaydaslar.Contains(daireId))
        {
            return Result<long>.Fail(
                "Sorumlu daire başkanlığı paydaş olarak eklenemez.");
        }

        var mevcutPaydaslar = entity.PaydasBirimler.ToList();

        //
        // DELETE
        //
        foreach (var dbItem in mevcutPaydaslar)
        {
            if (!requestPaydaslar.Contains(dbItem.DaireBaskanligiId))
            {
                entity.PaydasBirimler.Remove(dbItem);
            }
        }

        //
        // INSERT
        //
        foreach (var id in requestPaydaslar)
        {
            var existsInDb = entity.PaydasBirimler
                .Any(x => x.DaireBaskanligiId == id);

            if (!existsInDb)
            {
                entity.PaydasBirimler.Add(new ProjePaydasBirim
                {
                    DaireBaskanligiId = id
                });
            }
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
        //if (request.PaydasDaireBaskanligiIds.Contains(daireId))
        //{
        //    return Result<long>.Fail(
        //        "Sorumlu daire başkanlığı paydaş olarak eklenemez.");
        //}

        //entity.PaydasBirimler = request.PaydasDaireBaskanligiIds
        //    .Distinct()
        //    .Select(x => new ProjePaydasBirim
        //    {
        //        DaireBaskanligiId = x
        //    })
        //    .ToList();

        // Toplam hesap
        entity.ToplamBedel = entity.Bedeli + entity.IlaveSozlesmeBedeli;

        var dagilimToplam = entity.IlceDagilimlari
            .Where(x => !x.Silindi)
            .Sum(x => x.IlceyeOdenenBedeli);

        if (Math.Abs(dagilimToplam - entity.ToplamBedel) > 0.01m)
            return Result<long>.Fail(
                "İlçe dağılım toplamı proje toplamına eşit olmalıdır");


        // ================= FAALİYET ALANLARI =================

        var requestFaaliyetler =
            request.FaaliyetAlanlari ?? [];

        // Faaliyetlerde kullanılan ilçeler,
        // proje ilçe dağılımlarında bulunmalı
        var ilceDagilimiIlceleri = entity.IlceDagilimlari
            .Select(x => x.IlceId)
            .ToHashSet();

        var gecersizIlceler = requestFaaliyetler
            .Where(x => !ilceDagilimiIlceleri.Contains(x.IlceId))
            .Select(x => x.IlceId)
            .Distinct()
            .ToList();

        if (gecersizIlceler.Any())
        {
            return Result<long>.Fail(
                $"Faaliyetlerde kullanılan ilçeler dağılım listesinde bulunmalıdır. İlçeId: {string.Join(", ", gecersizIlceler)}");
        }

        // Mevcut faaliyetleri sil
        var mevcutFaaliyetler = entity.IlceDagilimlari
            .SelectMany(x => x.FaaliyetAlanlari)
            .ToList();

        foreach (var faaliyet in mevcutFaaliyetler)
        {
            uow.Repository<ProjeFaaliyetAlani>()
                .Remove(faaliyet);
        }

        // Yenilerini ekle
        foreach (var faaliyet in requestFaaliyetler)
        {
            var ilceDagilimi = entity.IlceDagilimlari
                .FirstOrDefault(x => x.IlceId == faaliyet.IlceId);

            if (ilceDagilimi == null)
                continue;

            ilceDagilimi.FaaliyetAlanlari.Add(
                new ProjeFaaliyetAlani
                {
                    Yil = faaliyet.Yil,
                    Ay = faaliyet.Ay,
                    KategoriDegerId = faaliyet.KategoriDegerId,
                    FaaliyetMiktari = faaliyet.FaaliyetMiktari
                });
        }

        var duplicateFaaliyet = requestFaaliyetler
        .GroupBy(x => new
        {
            x.IlceId,
            x.KategoriDegerId,
            x.Yil,
            x.Ay
        })
        .Any(g => g.Count() > 1);

            if (duplicateFaaliyet)
            {
                return Result<long>.Fail(
                    "Aynı ilçe, yıl, ay ve faaliyet alanı için birden fazla kayıt girilemez.");
            }

        await uow.SaveAsync(cancellationToken);

        return Result<long>.Ok(entity.Id, "Proje başarıyla güncellendi");
    }
}



