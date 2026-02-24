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

public class UpdateDegerCommandHandler(
    IUnitOfWork uow
) : IRequestHandler<UpdateDegerCommand, Result<long>>
{
    public async Task<Result<long>> Handle(
        UpdateDegerCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await uow.Repository<Deger>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (entity == null || entity.Silindi)
            return Result<long>.Fail("Değer bulunamadı");

        entity.Adi = request.Adi;
        entity.Kodu = request.Kodu;
        entity.SiraNo = request.SiraNo;

        await uow.SaveAsync(cancellationToken);

        return Result<long>.Ok(entity.Id, "Değer güncellendi");
    }
}
