namespace RebtelAssignment.Application.Core.Insights.Dtos;

public class TopMembersByLoanResponseDto
{
    public long MemberId { get; set; }
    public required string DisplayName { get; set; }
    public int LoanCount { get; set; }
}