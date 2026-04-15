using Application.Commands;
using Application.Helpers;
using Application.Services;
using Domain.Entities.FileMinio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers;

public class DeleteFilesByEntityCommandHandler(
    IUnitOfWork uow,
    IMinioService minioService
) : IRequestHandler<DeleteFilesByEntityCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteFilesByEntityCommand request, CancellationToken cancellationToken)
    {
        var fileReferences = await uow.Repository<FileReference>()
            .Query()
            .Where(x =>
                x.EntityId == request.EntityId &&
                x.EntityName == request.EntityName &&
                !x.Silindi)
            .ToListAsync(cancellationToken);

        if (!fileReferences.Any())
            return Result<bool>.Fail("Silinecek dosya bulunamadı");

        string warningMessage = null;

        foreach (var fileReference in fileReferences)
        {
            // ✅ Soft delete
            fileReference.Silindi = true;

            var fileEntity = await uow.Repository<FileEntity>()
                .Query()
                .FirstOrDefaultAsync(x => x.Id == fileReference.FileId, cancellationToken);

            if (fileEntity != null)
            {
                try
                {
                    await minioService.DeleteAsync(fileEntity.ObjectName);
                }
                catch
                {
                    warningMessage = "Bazı dosyalar storage'dan silinemedi.";
                }
            }
        }

        await uow.SaveAsync();

        if (!string.IsNullOrEmpty(warningMessage))
            return Result<bool>.Ok(true, warningMessage);

        return Result<bool>.Ok(true, "Tüm dosyalar silindi");
    }
}
