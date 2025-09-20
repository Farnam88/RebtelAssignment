using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RebTelAssignment.Domain.Models;
using RebtelAssignment.Infrastructure.Data.EntityConfigs.Converters;

namespace RebtelAssignment.Infrastructure.Data.EntityConfigs;

public class BookConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        EntityConfigHelper.SetAuditColumns(builder);

        builder.Property(p => p.Title)
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(1000);

        builder.Property(e => e.Authors)
            .IsRequired()
            .HasConversion(CustomConverters.StringListToJson);

        builder.Property(e => e.Subjects)
            .IsRequired()
            .HasConversion(CustomConverters.StringListToJson);
    }
}