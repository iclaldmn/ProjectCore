using Domain.Entities.Ortak;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FluentApi.Ortak
{
    public  class DegerFluentApi : IEntityTypeConfiguration<Deger>
    
    {
        public void Configure(EntityTypeBuilder<Deger> builder)
        {
            builder.ToTable(nameof(Deger),"Ortak");

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

            // Many Deger → One Kategori
            builder.HasOne(d => d.Kategori)
                   .WithMany(k => k.Degerler)
                   .HasForeignKey(d => d.KategoriId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
               // ---- Proje Tipi (KategoriId = 1)
               new Deger { Id = 1, Adi = "Yol", KategoriId = 1, Kodu = "YOL", SiraNo = 1 },
               new Deger { Id = 2, Adi = "Asfalt", KategoriId = 1, Kodu = "ASF", SiraNo = 2 },
               new Deger { Id = 3, Adi = "Bina", KategoriId = 1, Kodu = "BNA", SiraNo = 3 },

               // ---- Proje Durumu (KategoriId = 2)
               new Deger { Id = 4, Adi = "Tamamlandı", KategoriId = 2, Kodu = "TMMD", SiraNo = 1 },
               new Deger { Id = 5, Adi = "Devam Ediyor", KategoriId = 2, Kodu = "DEV", SiraNo = 2 },

               // ---- Ihale Türü (KategoriId = 3)
               new Deger { Id = 6, Adi = "Açık İhale", KategoriId = 3, Kodu = "ACK", SiraNo = 1 },
               new Deger { Id = 7, Adi = "DMO", KategoriId = 3, Kodu = "DMO", SiraNo = 2 },

               // ---- Hedef Kitlesi (KategoriId = 4)
               new Deger { Id = 8, Adi = "Vatandaş", KategoriId = 4, Kodu = "VTN", SiraNo = 1 },
               new Deger { Id = 9, Adi = "Personel", KategoriId = 4, Kodu = "PRS", SiraNo = 2 }
           );
        }
    }
}
