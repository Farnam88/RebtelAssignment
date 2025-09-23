using RebTelAssignment.Domain.Models.Enums;

namespace RebtelAssignment.Application.Common.DataTransferObjects;

public class LoanItemDto
{
    public long BatchId { get; set; }
    public long BookId { get; set; }
    public LoanItemStatus ItemStatus { get; set; }
    public DateTime? ReturnedAt { get; set; }
}
