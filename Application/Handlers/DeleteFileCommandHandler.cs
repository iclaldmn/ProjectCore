using Application.Commands;
using Application.Helpers;
using Application.Services;
using Domain.Entities.FileMinio;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers;

//tekli dosya işlemleri için geçerli olan command handler
public class DeleteFileCommandHandler(
    IUnitOfWork uow,
    IMinioService minioService
) : IRequestHandler<DeleteFileCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        var fileReference = await uow.Repository<FileReference>()
            .Query()
            .FirstOrDefaultAsync(x => x.Id == request.FileReferenceId, cancellationToken);

        if (fileReference == null || fileReference.Silindi)
            return Result<bool>.Fail("Dosya bulunamadı");

        // ✅ Soft delete
        fileReference.Silindi = true;

        string warningMessage = null;

        // FileEntity bul
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
                // ⚠️ sadece mesajı set et
                warningMessage = "Dosya sistemden silindi ancak storage'dan silinemedi.";
            }
        }

        await uow.SaveAsync();

        // ✅ Eğer warning varsa onu dön
        if (!string.IsNullOrEmpty(warningMessage))
            return Result<bool>.Ok(true, warningMessage);

        return Result<bool>.Ok(true, "Dosya silindi");
    }
}