using Domain.Entities.ProjeModul;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.FluentApi.ProjeModul;

using Domain.Entities.ProjeModul;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProjeFluentApi : IEntityTypeConfiguration<Proje>
{
    public void Configure(EntityTypeBuilder<Proje> builder)
    {
        builder.ToTable(nameof(Proje), "Proje");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Adi)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.Aciklama)
            .HasMaxLength(2000);

        builder.Property(x => x.Bedeli)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.IlaveSozlesmeBedeli)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.ToplamBedel)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.BaslangicTarihi)
            .IsRequired();

        builder.Property(x => x.BitisTarihi)
            .IsRequired();

        // 🔥 İlçe dağılım ilişkisi
        builder.HasMany(p => p.IlceDagilimlari)
               .WithOne(d => d.Proje)
               .HasForeignKey(d => d.ProjeId)
               .OnDelete(DeleteBehavior.Cascade);

        // 🔥 Dinamik kategori-değer ilişkisi
        builder.HasMany(p => p.KategoriDegerleri)
               .WithOne(k => k.Proje)
               .HasForeignKey(k => k.ProjeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.SorumluDaireBaskanligi)
                .WithMany()
                .HasForeignKey(x => x.SorumluDaireBaskanligiId)
                .OnDelete(DeleteBehavior.Restrict);
    }
}
