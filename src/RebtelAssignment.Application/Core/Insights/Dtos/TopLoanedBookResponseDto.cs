namespace RebtelAssignment.Application.Core.Insights.Dtos;

public class TopLoanedBookResponseDto
{
    public long BookId { get; set; }
    public required string Title { get; set; }
    public int Count { get; set; }
}