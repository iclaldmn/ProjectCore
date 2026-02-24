using Application.Commands;
using Application.Helpers;
using AutoMapper;
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

public class CreateDegerCommandHandler(
    IUnitOfWork uow,
    IMapper mapper
) : IRequestHandler<CreateDegerCommand, Result<long>>
{
    public async Task<Result<long>> Handle(
        CreateDegerCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await uow.Repository<Deger>()
            .Query()
            .AnyAsync(x =>
                !x.Silindi &&
                x.KategoriId == request.KategoriId &&
                (x.Adi == request.Adi || x.Kodu == request.Kodu),
                cancellationToken);

        if (exists)
            return Result<long>.Fail("Aynı kategori içinde bu değer zaten mevcut");

        var entity = mapper.Map<Deger>(request);

        await uow.Repository<Deger>().AddAsync(entity);
        await uow.SaveAsync(cancellationToken);

        return Result<long>.Ok(entity.Id, "Değer başarıyla eklendi");
    }
}