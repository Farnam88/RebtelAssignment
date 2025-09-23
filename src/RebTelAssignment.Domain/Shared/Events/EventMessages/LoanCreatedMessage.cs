using RebTelAssignment.Domain.Models.Enums;

namespace RebTelAssignment.Domain.Shared.Events.EventMessages;

/// <summary>
/// The consumer of this event could be email/sms service, CRM, Potential restocking of a batch and, etc.
/// </summary>
public class LoanCreatedMessage : IDomainMessage
{
    //must be string but, I kept it as long for simplicity
    public long LoanId { get; init; }
    //must be string but, I kept it as long for simplicity
    public long MemberId { get; init; }

    public required List<LoanItemMessage> LoanItems { get; set; }
    //Other necessary fields
}

public class LoanReturnedMessage : IDomainMessage
{
    public long LoanId { get; init; }
    //must be string but, I kept it as long for simplicity
    public long MemberId { get; init; }
    public required List<LoanItemMessage> LoanItems { get; set; }
    
}

public class LoanItemMessage
{
    public long BatchId { get; set; }
    public LoanItemStatus ItemStatus { get; set; }
}