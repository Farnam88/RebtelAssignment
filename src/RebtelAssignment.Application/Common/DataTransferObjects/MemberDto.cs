namespace RebtelAssignment.Application.Common.DataTransferObjects;

public class MemberDto
{
    public long Id { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public required string DisplayName { get; set; }
}