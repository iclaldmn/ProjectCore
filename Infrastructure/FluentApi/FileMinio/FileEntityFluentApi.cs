using Domain.Entities.FileMinio;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FluentApi.FileMinio;

public class FileEntityFluentApi : IEntityTypeConfiguration<FileEntity>
{
    public void Configure(EntityTypeBuilder<FileEntity> builder)
    {
        builder.ToTable(nameof(FileEntity), "Ortak");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FileName)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(x => x.ObjectName)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(x => x.Bucket)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.ContentType)
               .HasMaxLength(100);

        builder.Property(x => x.Size)
               .IsRequired();

        builder.Property(x => x.CreatedAt)
               .IsRequired();

        builder.Property(x => x.UploadedBy)
               .HasMaxLength(100);

        // Index (performans için)
        builder.HasIndex(x => x.ObjectName);

        // Relation
        builder.HasMany(x => x.References)
               .WithOne(x => x.File)
               .HasForeignKey(x => x.FileId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}