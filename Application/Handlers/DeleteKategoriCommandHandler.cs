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

public class DeleteKategoriCommandHandler(
    IUnitOfWork uow
) : IRequestHandler<DeleteKategoriCommand, Result<long>>
{
    public async Task<Result<long>> Handle(
        DeleteKategoriCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await uow.Repository<Kategori>()
            .Query()
            .Include(x => x.Degerler)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null || entity.Silindi)
            return Result<long>.Fail("Kategori bulunamadı");

        var kullaniliyorMu = entity.Degerler.Any(x => !x.Silindi);

        if (kullaniliyorMu)
            return Result<long>.Fail("Bu kategoriye bağlı değerler olduğu için silinemez");

        entity.Silindi = true;

        await uow.SaveAsync(cancellationToken);

        return Result<long>.Ok(entity.Id, "Kategori silindi");
    }
}