namespace RebTelAssignment.Domain.Shared.Events;

public interface IEventPublisher<T> where T : class, IDomainMessage
{
    Task Publish(T message, CancellationToken ct = default);
}