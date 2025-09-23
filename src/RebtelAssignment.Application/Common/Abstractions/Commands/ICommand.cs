using MediatR;
using RebTelAssignment.Domain.Shared.DataWrapper;

namespace RebtelAssignment.Application.Common.Abstractions.Commands;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}

public abstract record BaseCommand<T> : ICommand<ResultModel<T>>
{
}