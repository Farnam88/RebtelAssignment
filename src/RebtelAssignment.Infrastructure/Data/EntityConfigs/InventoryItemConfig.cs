using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RebTelAssignment.Domain.Models;

namespace RebtelAssignment.Infrastructure.Data.EntityConfigs;

public class InventoryItemConfig : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.Property(p => p.BatchId)
            .IsRequired();

        builder.Property(p => p.BookId)
            .IsRequired();

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

        builder.Property(p => p.BatchId)
            .IsRequired();

        builder.Ignore(p => p.QuantityAvailable)
            
            /*QuantityAvailable should be a computed field but, it does not work on in-memory database
        builder.Property(p => p.QuantityAvailable)
            .HasComputedColumnSql(
                $"{nameof(InventoryItem.Quantity)}-{nameof(InventoryItem.QuantityLoaned)}-" +
                $"{nameof(InventoryItem.QuantityDamaged)}-{nameof(InventoryItem.QuantityMissing)}",
                true)
            .ValueGeneratedOnAddOrUpdate()*/;
        
        /*
        This works only when it not an in-memory database and, it prevents negative numbers in columns such as
        Quantity, QuantityLoaned, QuantityDamaged, QuantityMissing and, QuantityAvailable;
        builder.ToTable("InventoryItems", t =>
        {
            //Non negative 
            t.HasCheckConstraint(
                "CK_inventory_item_consistent",
                $"{nameof(InventoryItem.Quantity)} >= 0 AND {nameof(InventoryItem.QuantityLoaned)}  >= 0 AND " +
                $"{nameof(InventoryItem.QuantityDamaged)}  >= 0 AND {nameof(InventoryItem.QuantityMissing)}  >= 0 AND ");

            //Non negative availability
            t.HasCheckConstraint(
                "CK_inventory_item_available_nonneg",
                $"({nameof(InventoryItem.Quantity)}-{nameof(InventoryItem.QuantityLoaned)}-" +
                $"{nameof(InventoryItem.QuantityDamaged)}-{nameof(InventoryItem.QuantityMissing)}) >= 0");
        });
        */
        
        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.LastModifiedAt)
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnUpdate();

        builder.Property(p => p.RowVersion)
            .IsRequired()
            .IsRowVersion();

        builder.HasOne<Batch>(b => b.Batch)
            .WithMany(m => m.InventoryItem)
            .HasForeignKey(f => f.BatchId);

        builder.HasOne<Book>(b => b.Book)
            .WithMany(m => m.InventoryItems)
            .HasForeignKey(f => f.BookId);
    }
}