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

        entity.Adi = request.Adi;

        await uow.SaveAsync(cancellationToken);

        return Result<long>.Ok(entity.Id, "Kategori güncellendi");
    }
}
