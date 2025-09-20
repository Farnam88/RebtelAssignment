using RebTelAssignment.Domain.Models.BaseModels;

namespace RebTelAssignment.Domain.Models;

public class Batch : SimpleAuditEntity
{
    public Batch()
    {
        LoanItems = new HashSet<LoanItem>();
    }

    public long BookId { get; set; }
    public required string Isbn { get; set; }
    public required string Publisher { get; set; }
    public DateOnly PublishedDate { get; set; }
    public int Edition { get; set; }
    public required long Pages { get; set; }

    public virtual Book Book { get; set; }
    public virtual ICollection<LoanItem> LoanItems { get; set; }
    public virtual ICollection<InventoryItem> InventoryItem { get; set; }

    //future extensions of the Batch
    // public string? Section { get; set; }
    // public string? Rack { get; set; }
    // public string? Position { get; set; }
    // public decimal? Weight { get; set; }
    // public decimal? Height { get; set; }
    // public decimal? Lenght { get; set; }
    // public decimal? Width { get; set; }
    //public decimal? Volume { get; set; }
}