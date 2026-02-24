using Application.Commands;
using Application.Helpers;
using Domain.Entities.Ortak;
using MediatR;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers;

using Domain.Entities.ProjeModul;
using Microsoft.EntityFrameworkCore;

public class DeleteDegerCommandHandler(
    IUnitOfWork uow
) : IRequestHandler<DeleteDegerCommand, Result<long>>
{
    public async Task<Result<long>> Handle(
        DeleteDegerCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await uow.Repository<Deger>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (entity == null || entity.Silindi)
            return Result<long>.Fail("Değer bulunamadı");

        // 🔥 1️⃣ Projelerde kullanılıyor mu kontrolü
        var kullaniliyorMu = await uow.Repository<Proje>()
            .Query()
            .AnyAsync(p =>
                !p.Silindi &&
                (
                    p.ProjeTipiId == entity.Id ||
                    p.ProjeDurumuId == entity.Id ||
                    p.IhaleTuruId == entity.Id ||
                    p.HedefKitleId == entity.Id
                ),
                cancellationToken);

        if (kullaniliyorMu)
            return Result<long>.Fail("Bu değer projelerde kullanıldığı için silinemez.");

        // 🔥 2️⃣ Soft delete
        entity.Silindi = true;

        await uow.SaveAsync(cancellationToken);

        return Result<long>.Ok(entity.Id, "Değer başarıyla silindi");
    }
}
