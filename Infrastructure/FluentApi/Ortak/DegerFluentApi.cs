using Domain.Entities.Ortak;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FluentApi.Ortak;

public class DegerFluentApi : IEntityTypeConfiguration<Deger>
{
    public void Configure(EntityTypeBuilder<Deger> builder)
    {
        builder.ToTable(nameof(Deger), "Ortak");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Adi)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(x => x.Kodu)
               .HasMaxLength(50);

        builder.Property(x => x.SiraNo)
               .IsRequired();

        builder.Property(x => x.KategoriId)
               .IsRequired();

        // Index
        builder.HasIndex(x => x.KategoriId);

        // Optional: Aynı kategori içinde isim unique olsun
        builder.HasIndex(x => new { x.KategoriId, x.Adi })
               .IsUnique();

        // Many Deger → One Kategori
        builder.HasOne(d => d.Kategori)
               .WithMany(k => k.Degerler)
               .HasForeignKey(d => d.KategoriId)
               .OnDelete(DeleteBehavior.Restrict);

        // Optional Soft Delete
        // builder.HasQueryFilter(x => !x.Silindi);
    }
}