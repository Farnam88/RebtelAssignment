namespace RebtelAssignment.Application.Common.DataTransferObjects;

public class BatchDto
{
    public long Id { get; set; }
    public long BookId { get; set; }
    public required string Isbn { get; set; }
    public required string Publisher { get; set; }
    public DateOnly PublishedDate { get; set; }
    public int Edition { get; set; }
    public required long Pages { get; set; }
    public long AvailableQuantity { get; set; }
}