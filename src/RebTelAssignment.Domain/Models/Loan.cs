using RebTelAssignment.Domain.Models.BaseModels;

namespace RebTelAssignment.Domain.Models;

public class Loan : SimpleAuditEntity
{
    public Loan()
    {
        LoanItems = new HashSet<LoanItem>();
    }

    public required long MemberId { get; set; }
    public DateOnly LoanedAt { get; set; }
    public DateOnly DueAt { get; set; }
    public virtual Member Member { get; set; }
    public virtual ICollection<LoanItem> LoanItems { get; set; }
}