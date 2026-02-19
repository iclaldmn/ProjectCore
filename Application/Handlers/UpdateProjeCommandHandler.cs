using Application.Commands;
using Application.Validators;
using AutoMapper;
using Domain.Entities.ProjeModul;
using MediatR;
using Repository.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace Application.Handlers;

public class UpdateProjeCommandHandler(
    IUnitOfWork uow,
    IMapper mapper
) : IRequestHandler<UpdateProjeCommand, long>
{
    public async Task<long> Handle(
        UpdateProjeCommand request,
        CancellationToken cancellationToken)
    {
        // 1️⃣ Proje + İlçe dağılımları dahil yükle
        var entity = await uow.Repository<Proje>()
            .GetWithIncludeAsync(
                x => x.Id == request.Id,
                cancellationToken,
                x => x.IlceDagilimlari
            );

        if (entity == null)
            throw new Exception("Proje bulunamadı");

        // 2️⃣ Scalar alanları map et
        entity.Adi = request.Adi;
        entity.Aciklama = request.Aciklama;
        entity.Bedeli = request.Bedeli;
        entity.IlaveSozlesmeBedeli = request.IlaveSozlesmeBedeli;
        entity.IhaleTuruId = request.IhaleTuruId;
        entity.HedefKitleId = request.HedefKitleId;
        entity.ProjeTipiId = request.ProjeTipiId;
        entity.ProjeDurumuId = request.ProjeDurumuId;
        entity.BaslangicTarihi = request.BaslangicTarihi;
        entity.BitisTarihi = request.BitisTarihi;

        // 3️⃣ İlçe dağılımları null güvenliği
        var ilceList = request.IlceDagilimlari
            ?? new List<UpdateProjeIlceDagilimiCommand>();

        // 4️⃣ Eski dağılımları sil
        entity.IlceDagilimlari.Clear();

        // 5️⃣ Yenilerini ekle
        foreach (var item in ilceList)
        {
            entity.IlceDagilimlari.Add(new ProjeIlceDagilimi
            {
                IlceId = item.IlceId,
                IlceyeOdenenBedeli = item.IlceyeOdenenBedeli
            });
        }

        // 6️⃣ Toplam hesap
        entity.ToplamBedel = entity.Bedeli + entity.IlaveSozlesmeBedeli;

        var dagilimToplam = entity.IlceDagilimlari
            .Sum(x => x.IlceyeOdenenBedeli);

        if (dagilimToplam != entity.ToplamBedel)
            throw new Exception("İlçe dağılım toplamı proje toplamına eşit olmalıdır");

        // 7️⃣ Save
        await uow.SaveAsync(cancellationToken);

        return entity.Id;
    }
}



