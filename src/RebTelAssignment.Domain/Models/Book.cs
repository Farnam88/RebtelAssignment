using RebTelAssignment.Domain.Models.BaseModels;

namespace RebTelAssignment.Domain.Models;

public class Book : SimpleAuditEntity
{
    public Book()
    {
        Batches = new HashSet<Batch>();
        LoanItems = new HashSet<LoanItem>();
        InventoryItems = new HashSet<InventoryItem>();
    }

    public required string Title { get; set; }
    public required List<string> Authors { get; set; }
    public required List<string> Subjects { get; set; }
    public virtual ICollection<Batch> Batches { get; set; }
    public virtual ICollection<LoanItem> LoanItems { get; set; }
    public virtual ICollection<InventoryItem> InventoryItems { get; set; }
}