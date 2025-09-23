using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RebTelAssignment.Domain.Models.BaseModels;

namespace RebtelAssignment.Infrastructure.Data.EntityConfigs;

public static class EntityConfigHelper
{
    public static void SetAuditColumns<T>(EntityTypeBuilder<T> builder) where T : SimpleAuditEntity
    {
        builder.HasKey(e => e.Id);
        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.LastModifiedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnUpdate();
    }
}