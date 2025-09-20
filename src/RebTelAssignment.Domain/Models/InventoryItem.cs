using RebTelAssignment.Domain.Models.BaseModels;

namespace RebTelAssignment.Domain.Models;

public class InventoryItem : ICreatedEntity, IModifiedEntity, IConcurrentEntity
{
    public long BatchId { get; set; }
    public long BookId { get; set; }
    public required long Quantity { get; set; }
    public required long QuantityLoaned { get; set; }
    public required long QuantityDamaged { get; set; }
    public required long QuantityMissing { get; set; }
    public long QuantityAvailable => Quantity - QuantityLoaned - QuantityDamaged - QuantityMissing;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public required byte[] RowVersion { get; set; }
    public virtual Book Book { get; set; }
    public virtual Batch Batch { get; set; }
}