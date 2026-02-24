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

public class CreateKategoriCommandHandler(
    IUnitOfWork uow
) : IRequestHandler<CreateKategoriCommand, Result<long>>
{
    public async Task<Result<long>> Handle(
        CreateKategoriCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await uow.Repository<Kategori>()
            .Query()
            .AnyAsync(x => x.Adi == request.Adi && !x.Silindi, cancellationToken);

        if (exists)
            return Result<long>.Fail("Bu kategori zaten mevcut");

        var entity = new Kategori
        {
            Adi = request.Adi
        };

        await uow.Repository<Kategori>().AddAsync(entity);
        await uow.SaveAsync(cancellationToken);

        return Result<long>.Ok(entity.Id, "Kategori oluşturuldu");
    }
}
