namespace RebTelAssignment.Domain.Shared.Events;

public interface IDomainMessage
{
    public Guid EventId => Guid.NewGuid();
}