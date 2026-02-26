using Application.Commands;
using Application.Helpers;
using Domain.Entities.Ortak;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers;

public class UpdateKategoriCommandHandler(
    IUnitOfWork uow
) : IRequestHandler<UpdateKategoriCommand, Result<long>>
{
    public async Task<Result<long>> Handle(
        UpdateKategoriCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await uow.Repository<Kategori>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (entity == null || entity.Silindi)
            return Result<long>.Fail("Kategori bulunamadı");

        // 🔥 Duplicate kontrol (kendi kaydı hariç)
        var exists = await uow.Repository<Kategori>()
            .Query()
            .AnyAsync(x =>
                x.Adi == request.Adi &&
                x.Id != request.Id &&
                !x.Silindi,
                cancellationToken);

        if (exists)
            return Result<long>.Fail("Bu kategori adı zaten mevcut");

        // 🔥 Business rule
        if (request.ProjedeZorunlu && !request.ProjedeGoster)
            return Result<long>.Fail(
                "Zorunlu kategori projede gösterilmek zorundadır");

        // 🔥 Alanları güncelle
        entity.Adi = request.Adi;
        entity.Aktif = request.Aktif;
        entity.ProjedeGoster = request.ProjedeGoster;
        entity.ProjedeZorunlu = request.ProjedeZorunlu;

        await uow.SaveAsync(cancellationToken);

        return Result<long>.Ok(entity.Id, "Kategori güncellendi");
    }
}
