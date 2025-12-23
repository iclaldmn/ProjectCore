using Domain.Entities.Ortak;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FluentApi.Ortak
{
    public class KategoriFluentApi : IEntityTypeConfiguration<Kategori>
    {
        public void Configure(EntityTypeBuilder<Kategori> builder)
        {
            builder.ToTable(nameof(Kategori), "Ortak");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Adi)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasData(
             new Kategori { Id = 1, Adi = "Proje Tipi" },
             new Kategori { Id = 2, Adi = "Proje Durumu" },
             new Kategori { Id = 3, Adi = "İhale Türü" },
             new Kategori { Id = 4, Adi = "Hedef Kitlesi" }
         );
            // One Kategori → Many Degerler
            //builder.HasMany(k => k.Degerler)
            //       .WithOne(d => d.Kategori)
            //       .HasForeignKey(d => d.KategoriId)
            //       .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
