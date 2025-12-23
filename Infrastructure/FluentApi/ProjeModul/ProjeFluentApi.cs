using Domain.Entities.ProjeModul;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.FluentApi.ProjeModul
{
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

            builder.Property(x => x.BaslangicTarihi)
                .IsRequired();

            builder.Property(x => x.BitisTarihi)
                .IsRequired();

            // 🔹 Proje Tipi
            builder.HasOne(p => p.ProjeTipi)
                   .WithMany(d => d.ProjelerAsProjeTipi)
                   .HasForeignKey(p => p.ProjeTipiId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Proje Durumu
            builder.HasOne(p => p.ProjeDurumu)
                   .WithMany(d => d.ProjelerAsProjeDurumu)
                   .HasForeignKey(p => p.ProjeDurumuId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 🔹 İhale Türü
            builder.HasOne(p => p.IhaleTuru)
                   .WithMany(d => d.ProjelerAsIhaleTuru)
                   .HasForeignKey(p => p.IhaleTuruId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Hedef Kitle
            builder.HasOne(p => p.HedefKitle)
                   .WithMany(d => d.ProjelerAsHedefKitle)
                   .HasForeignKey(p => p.HedefKitleId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
