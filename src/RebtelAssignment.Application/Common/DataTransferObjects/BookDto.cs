namespace RebtelAssignment.Application.Common.DataTransferObjects;

public class BookDto
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public required List<string> Authors { get; set; }
    public required List<string> Subjects { get; set; }
}
