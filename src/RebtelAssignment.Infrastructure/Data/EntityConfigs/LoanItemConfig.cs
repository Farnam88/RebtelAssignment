using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RebTelAssignment.Domain.Models;
using RebTelAssignment.Domain.Models.Enums;

namespace RebtelAssignment.Infrastructure.Data.EntityConfigs;

public class LoanItemConfig : IEntityTypeConfiguration<LoanItem>
{
    public void Configure(EntityTypeBuilder<LoanItem> builder)
    {
        EntityConfigHelper.SetAuditColumns(builder);

        builder.Property(p => p.LoanId)
            .IsRequired();

        builder.Property(p => p.BatchId)
            .IsRequired();

        builder.Property(p => p.BookId)
            .IsRequired();

        builder.Property(p => p.ItemStatus)
            .IsRequired()
            .HasDefaultValue(LoanItemStatus.Loaned)
            .HasSentinel(LoanItemStatus.Unspecified)
            .HasConversion<int>();

        builder.Property(p => p.ReturnedAt);

        builder.Property(p => p.Comment)
            .HasMaxLength(4000)
            .IsUnicode();

        builder.HasOne(o => o.Batch)
            .WithMany(m => m.LoanItems)
            .HasForeignKey(f => f.BatchId);

        builder.HasOne(o => o.Book)
            .WithMany(m => m.LoanItems)
            .HasForeignKey(f => f.BookId);

        builder.HasOne(o => o.Loan)
            .WithMany(m => m.LoanItems)
            .HasForeignKey(f => f.LoanId);
    }
}