using MediatR;
using RebTelAssignment.Domain.Shared.DataWrapper;

namespace RebtelAssignment.Application.Common.Abstractions.Queries;

public interface IQuery<out TResponse> : IRequest<TResponse>;

/// <summary>
/// Mediator's <see cref="IQuery{TResponse}"/> which wraps TResponse with <see cref="ResultModel{TResult}"/> 
/// </summary>
/// <typeparam name="T">TResponse type</typeparam>
public abstract record BaseQuery<T> : IQuery<ResultModel<T>>
{    
}

