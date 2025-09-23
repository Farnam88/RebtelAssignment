using RebTelAssignment.Domain.Models.BaseModels;

namespace RebTelAssignment.Domain.Models;

public class Member : BaseEntity
{
    // public Member()
    // {
    //     Loans = new HashSet<Loan>();
    // }
    public required string DisplayName { get; set; }
    public virtual ICollection<Loan> Loans { get; set; }
}