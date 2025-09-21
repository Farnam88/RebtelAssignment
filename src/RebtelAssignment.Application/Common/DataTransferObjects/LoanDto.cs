namespace RebtelAssignment.Application.Common.DataTransferObjects;

public class LoanDto
{
    public long Id { get; set; }
    public required long MemberId { get; set; }
    public DateOnly LoanedAt { get; set; }
    public DateOnly DueAt { get; set; }
    public required IList<LoanItemDto> LoanItems { get; set; }
}