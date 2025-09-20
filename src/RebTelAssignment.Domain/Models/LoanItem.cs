using RebTelAssignment.Domain.Models.BaseModels;
using RebTelAssignment.Domain.Models.Enums;

namespace RebTelAssignment.Domain.Models;

public class LoanItem : SimpleAuditEntity
{
    public long LoanId { get; set; }
    public long BatchId { get; set; }
    public long BookId { get; set; }
    public LoanItemStatus ItemStatus { get; set; }
    public DateTime? ReturnedAt { get; set; }
    public string? Comment { get; set; }
    public virtual Batch Batch { get; set; }
    public virtual Book Book { get; set; }
    public virtual Loan Loan { get; set; }
}