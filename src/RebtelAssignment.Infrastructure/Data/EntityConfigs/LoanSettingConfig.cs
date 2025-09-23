using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RebTelAssignment.Domain.Models;
using RebTelAssignment.Domain.Models.Enums;

namespace RebtelAssignment.Infrastructure.Data.EntityConfigs;

public class LoanSettingConfig : IEntityTypeConfiguration<LoanSetting>
{
    public void Configure(EntityTypeBuilder<LoanSetting> builder)
    {
        EntityConfigHelper.SetAuditColumns(builder);

        builder.Property(p => p.LoanDurationUnitType)
            .IsRequired()
            .HasDefaultValue(LoanDurationUnitType.Day)
            .HasSentinel(LoanDurationUnitType.Unspecified)
            .HasConversion<int>();

        builder.Property(p => p.Value)
            .IsRequired()
            .HasDefaultValue(14);
    }
}