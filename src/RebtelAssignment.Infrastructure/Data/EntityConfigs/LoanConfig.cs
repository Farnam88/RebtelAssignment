using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RebTelAssignment.Domain.Models;
using RebtelAssignment.Infrastructure.Data.EntityConfigs.Converters;

namespace RebtelAssignment.Infrastructure.Data.EntityConfigs;

public class LoanConfig : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        EntityConfigHelper.SetAuditColumns(builder);

        builder.Property(p => p.LoanedAt)
            .IsRequired();

        builder.Property(p => p.DueAt)
            .IsRequired()
            .HasConversion(CustomConverters.DateOnlyConverter);


        builder.HasOne<Member>()
            .WithMany(m => m.Loans)
            .HasForeignKey(f => f.MemberId);
    }
}