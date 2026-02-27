using Application.Commands;
using Application.Helpers;
using Domain.Entities.ProjeModul;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers;

public class DeleteProjeCommandHandler(
    IUnitOfWork uow
) : IRequestHandler<DeleteProjeCommand, Result<long>>
{
    public async Task<Result<long>> Handle(
     DeleteProjeCommand request,
     CancellationToken cancellationToken)
    {
        var entity = await uow.Repository<Proje>()
            .Query()
            .Include(x => x.IlceDagilimlari)
            .Include(x => x.KategoriDegerleri)   // 🔥 önemli
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null)
            return Result<long>.Fail("Proje bulunamadı");

        if (entity.Silindi)
            return Result<long>.Fail("Proje zaten silinmiş");

        // 🔥 Parent soft delete
        entity.Silindi = true;

        // 🔥 İlçe dağılımları soft delete
        foreach (var item in entity.IlceDagilimlari)
        {
            item.Silindi = true;
        }

        // 🔥 KategoriDeger soft delete
        foreach (var item in entity.KategoriDegerleri)
        {
            item.Silindi = true;
        }

        await uow.SaveAsync(cancellationToken);

        return Result<long>.Ok(entity.Id, "Proje başarıyla silindi");
    }
}