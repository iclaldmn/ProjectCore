using Domain.Entities.ProjeModul;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FluentApi.ProjeModul
{
    public class ProjeIlceDagilimiFluentApi : IEntityTypeConfiguration<ProjeIlceDagilimi>
    {
        public void Configure(EntityTypeBuilder<ProjeIlceDagilimi> builder)
        {
            builder.ToTable(nameof(ProjeIlceDagilimi), "Proje");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.IlceyeOdenenBedeli)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.IlceId)
                   .IsRequired();

            builder.Property(x => x.ProjeId)
                   .IsRequired();

            // 🔹 İlçe → ProjeIlceDagilimi (One-to-Many)
            builder.HasOne(x => x.Ilce)
                   .WithMany(i => i.IlceDagilimlari)
                   .HasForeignKey(x => x.IlceId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Proje → ProjeIlceDagilimi (One-to-Many)
            builder.HasOne(x => x.Proje)
                   .WithMany(p => p.IlceDagilimlari)
                   .HasForeignKey(x => x.ProjeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
