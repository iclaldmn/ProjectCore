using Application.Commands;
using Application.Helpers;
using Application.Services;
using Domain.Entities.FileMinio;
using Infrastructure.Context;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers;

public class UploadFileCommandHandler(
    IUnitOfWork uow,
    IMinioService minioService
) : IRequestHandler<UploadFileCommand, Result<List<Guid>>>
{
    public async Task<Result<List<Guid>>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        if (request.Files == null || !request.Files.Any())
            return Result<List<Guid>>.Fail("Dosya yok");

        var uploadedIds = new List<Guid>();
        var uploadedObjectNames = new List<string>();

        try
        {
            foreach (var file in request.Files)
            {
                if (file.Length == 0)
                    continue;

                var objectName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                using var stream = file.OpenReadStream();

                // 1️⃣ MinIO
                await minioService.UploadAsync(stream, objectName, file.ContentType);

                uploadedObjectNames.Add(objectName);

                // 2️⃣ FileEntity
                var fileEntity = new FileEntity
                {
                    Id = Guid.NewGuid(),
                    FileName = file.FileName,
                    ObjectName = objectName,
                    Bucket = "project-files",
                    ContentType = file.ContentType,
                    Size = file.Length,
                    CreatedAt = DateTime.UtcNow
                };

                await uow.Repository<FileEntity>().AddAsync(fileEntity);

                // 3️⃣ FileReference
                var fileReference = new FileReference
                {
                    Id = Guid.NewGuid(),
                    FileId = fileEntity.Id,
                    EntityId = request.EntityId,
                    EntityName = request.EntityName,
                    CreatedAt = DateTime.UtcNow,
                    Silindi = false
                };

                await uow.Repository<FileReference>().AddAsync(fileReference);

                uploadedIds.Add(fileEntity.Id);
            }

            await uow.SaveAsync();

            return Result<List<Guid>>.Ok(uploadedIds, "Dosyalar yüklendi");
        }
        catch (Exception ex)
        {
            // ❗ rollback (yüklenenleri sil)
            foreach (var obj in uploadedObjectNames)
            {
                try
                {
                    await minioService.DeleteAsync(obj);
                }
                catch { }
            }

            return Result<List<Guid>>.Fail($"Yükleme hatası: {ex.Message}");
        }
    }
}
