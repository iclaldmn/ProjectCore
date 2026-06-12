using Domain.Entities.ProjeModul;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Kullanici;

namespace Infrastructure.FluentApi.ProjeModul;

public class DaireBaskanligiConfiguration : IEntityTypeConfiguration<DaireBaskanligi>
{
    public void Configure(EntityTypeBuilder<DaireBaskanligi> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Adi)
            .IsRequired()
            .HasMaxLength(250);

        builder.HasMany(x => x.Kullanicilar)
            .WithOne(x => x.DaireBaskanligi)
            .HasForeignKey(x => x.DaireBaskanligiId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
