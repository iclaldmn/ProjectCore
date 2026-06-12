using Domain.Entities.ProjeModul;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FluentApi.ProjeModul;

public class ProjePaydasBirimConfiguration : IEntityTypeConfiguration<ProjePaydasBirim>
{
    public void Configure(EntityTypeBuilder<ProjePaydasBirim> builder)
    {
        builder.ToTable("ProjePaydasBirim", "Proje");
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Proje)
            .WithMany(x => x.PaydasBirimler)
            .HasForeignKey(x => x.ProjeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.DaireBaskanligi)
            .WithMany()
            .HasForeignKey(x => x.DaireBaskanligiId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.ProjeId,
            x.DaireBaskanligiId
        }).IsUnique();
    }
}
