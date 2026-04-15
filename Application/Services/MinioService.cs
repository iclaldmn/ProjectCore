using Minio;
using Minio.DataModel.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class MinioService : IMinioService
{
    private readonly IMinioClient _minio;
    private const string BucketName = "project-files";

    public MinioService()
    {
        _minio = new MinioClient()
            .WithEndpoint("localhost", 9002)  // 🔥 API PORT
            .WithCredentials("admin", "password123")
            .WithSSL(false)
            .Build();
    }

    public async Task<string> UploadAsync(Stream stream, string fileName, string contentType)
    {
        var objectName = Guid.NewGuid() + "_" + fileName;

        // 🔥 bucket var mı kontrol et
        var bucketExists = await _minio.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(BucketName)
        );

        if (!bucketExists)
        {
            await _minio.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(BucketName)
            );
        }

        stream.Position = 0;

        await _minio.PutObjectAsync(new PutObjectArgs()
            .WithBucket(BucketName)
            .WithObject(objectName)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType(contentType)
        );

        return objectName;
    }

    public async Task DeleteAsync(string objectName)
    {
        await _minio.RemoveObjectAsync(
            new RemoveObjectArgs()
                .WithBucket(BucketName)
                .WithObject(objectName)
        );
    }
}
