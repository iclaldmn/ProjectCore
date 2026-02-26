using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FluentApi.ProjeModul;

using Domain.Entities.Ortak;
using Domain.Entities.ProjeModul;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProjeKategoriDegerFluentApi
    : IEntityTypeConfiguration<ProjeKategoriDeger>
{
    public void Configure(EntityTypeBuilder<ProjeKategoriDeger> builder)
    {
        builder.ToTable(nameof(ProjeKategoriDeger), "Proje");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Proje)
            .WithMany(p => p.KategoriDegerleri)
            .HasForeignKey(x => x.ProjeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Kategori)
            .WithMany()
            .HasForeignKey(x => x.KategoriId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}