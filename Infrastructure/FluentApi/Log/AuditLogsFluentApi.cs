using Domain.Entities.Log;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FluentApi.Log;
    public class AuditLogsFluentApi : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .HasMaxLength(100);

            builder.Property(x => x.UserName)
                .HasMaxLength(100);

            builder.Property(x => x.Action)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.EntityName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.EntityId)
                .HasMaxLength(50);

            builder.Property(x => x.CreatedAt)
                .IsRequired();
        }
    }
 