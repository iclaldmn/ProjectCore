using Domain.Entities.ProjeModul;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FluentApi.ProjeModul;
      public class ProjeFaaliyetAlaniFluentApi
    : IEntityTypeConfiguration<ProjeFaaliyetAlani>
    {
        public void Configure(EntityTypeBuilder<ProjeFaaliyetAlani> builder)
        {
            builder.ToTable("ProjeFaaliyetAlanlari", "Proje");

            builder.Property(x => x.FaaliyetMiktari)
                .HasPrecision(18, 2);

            builder.HasOne(x => x.KategoriDeger)
                .WithMany(x => x.ProjeFaaliyetAlanlari)
                .HasForeignKey(x => x.KategoriDegerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.IlceDagilimi)
                .WithMany(x => x.FaaliyetAlanlari)
                .HasForeignKey(x => x.IlceDagilimiId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new
            {
                x.IlceDagilimiId,
                x.KategoriDegerId,
                x.Yil,
                x.Ay
            }).IsUnique();
        }
      }