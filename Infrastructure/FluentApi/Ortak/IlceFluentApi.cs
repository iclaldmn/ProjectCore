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
    public class IlceFluentApi : IEntityTypeConfiguration<Ilce>
    {
        public void Configure(EntityTypeBuilder<Ilce> builder)
        {
            // 🔹 Tablo adı ve şeması
            builder.ToTable(nameof(Ilce), "Ortak");

            // 🔹 Primary Key
            builder.HasKey(x => x.Id);

            // 🔹 Adi
            builder.Property(x => x.Adi)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasData(
                new Ilce { Id = 1, Adi = "Akyurt" },
                new Ilce { Id = 2, Adi = "Altındağ" },
                new Ilce { Id = 3, Adi = "Ayaş" },
                new Ilce { Id = 4, Adi = "Bala" },
                new Ilce { Id = 5, Adi = "Beypazarı" },
                new Ilce { Id = 6, Adi = "Çamlıdere" },
                new Ilce { Id = 7, Adi = "Çankaya" },
                new Ilce { Id = 8, Adi = "Çubuk" },
                new Ilce { Id = 9, Adi = "Elmadağ" },
                new Ilce { Id = 10, Adi = "Etimesgut" },
                new Ilce { Id = 11, Adi = "Evren" },
                new Ilce { Id = 12, Adi = "Gölbaşı" },
                new Ilce { Id = 13, Adi = "Güdül" },
                new Ilce { Id = 14, Adi = "Haymana" },
                new Ilce { Id = 15, Adi = "Kahramankazan" },
                new Ilce { Id = 16, Adi = "Kalecik" },
                new Ilce { Id = 17, Adi = "Keçiören" },
                new Ilce { Id = 18, Adi = "Kızılcahamam" },
                new Ilce { Id = 19, Adi = "Mamak" },
                new Ilce { Id = 20, Adi = "Nallıhan" },
                new Ilce { Id = 21, Adi = "Polatlı" },
                new Ilce { Id = 22, Adi = "Pursaklar" },
                new Ilce { Id = 23, Adi = "Sincan" },
                new Ilce { Id = 24, Adi = "Şereflikoçhisar" },
                new Ilce { Id = 25, Adi = "Yenimahalle" }
            );
        }
    }
}
