using Domain.Entities.FileMinio;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FluentApi.FileMinio;

public class FileReferenceFluentApi : IEntityTypeConfiguration<FileReference>
{
    public void Configure(EntityTypeBuilder<FileReference> builder)
    {
        builder.ToTable(nameof(FileReference), "Ortak");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EntityId)
               .IsRequired();

        builder.Property(x => x.EntityName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.CreatedAt)
               .IsRequired();

        // Index (çok önemli ⚡)
        builder.HasIndex(x => new { x.EntityId, x.EntityName });

        builder.HasIndex(x => x.FileId);
        builder.Property(x => x.Silindi)
       .HasDefaultValue(false);

        // Relation
        builder.HasOne(x => x.File)
               .WithMany(x => x.References)
               .HasForeignKey(x => x.FileId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
