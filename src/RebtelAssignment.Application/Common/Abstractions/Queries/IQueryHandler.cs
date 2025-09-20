using MediatR;
using RebtelAssignment.Application.Data;
using RebTelAssignment.Domain.Shared.DataWrapper;

namespace RebtelAssignment.Application.Common.Abstractions.Queries;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}

public abstract class BaseQueryHandler<TQuery, TResponse>
    : IQueryHandler<TQuery, ResultModel<TResponse>>
    where TQuery : BaseQuery<TResponse>
{
    protected readonly IUnitOfWork Uow;

    protected BaseQueryHandler(IUnitOfWork unitOfWork)
    {
        Uow = unitOfWork;
    }
    public abstract Task<ResultModel<TResponse>> Handle(TQuery request, CancellationToken ct = default);
}