namespace RebtelAssignment.Application.Core.Insights.Dtos;

public class MemberReadingPaceResponseDto
{
    public decimal PagesPerDay { get; set; }
    public required List<ReadingPaceByBookResponseDto> Books { get; set; }
}

public class ReadingPaceByBookResponseDto
{
    public long BookId { get; set; }
    public required string Title { get; set; }
    public decimal PagesPerDay { get; set; }
}