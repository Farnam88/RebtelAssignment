using RebTelAssignment.Domain.Shared.Events;

namespace RebtelAssignment.Application.Core.EventPublishers;

public class EventPublisher<T> : IEventPublisher<T> where T : class, IDomainMessage
{
    public Task Publish(T message, CancellationToken ct = default)
    {
        return Task.CompletedTask;
    }
}