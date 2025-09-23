using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RebTelAssignment.Domain.Models;
using RebtelAssignment.Infrastructure.Data.EntityConfigs.Converters;

namespace RebtelAssignment.Infrastructure.Data.EntityConfigs;

public class BatchConfig : IEntityTypeConfiguration<Batch>
{
    public void Configure(EntityTypeBuilder<Batch> builder)
    {
        EntityConfigHelper.SetAuditColumns(builder);

        builder.Property(p => p.Isbn)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(p => p.Publisher)
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(1000);

        builder.Property(p => p.PublishedDate)
            .IsRequired()
            .HasConversion(CustomConverters.DateOnlyConverter);

        builder.Property(p => p.Edition)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(p => p.Pages)
            .IsRequired();
        
        builder.Property(p => p.RowVersion)
            .IsRowVersion()
            .ValueGeneratedOnAddOrUpdate();
        builder.Property(p => p.Quantity)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(p => p.QuantityLoaned)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(p => p.QuantityDamaged)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(p => p.QuantityMissing)
            .IsRequired()
            .HasDefaultValue(0);


        builder.HasOne<Book>(b => b.Book)
            .WithMany(m => m.Batches)
            .HasForeignKey(f => f.BookId);
    }
}