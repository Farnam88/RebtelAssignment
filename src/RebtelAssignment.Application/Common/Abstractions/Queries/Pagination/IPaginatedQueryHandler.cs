using MediatR;
using RebtelAssignment.Application.Data;
using RebTelAssignment.Domain.Shared.DataWrapper;

namespace RebtelAssignment.Application.Common.Abstractions.Queries.Pagination;

public interface IPaginatedQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IPaginatedQuery<TResponse>
{
}

public abstract class BasePaginatedQueryHandler<TQuery, TResponse>
    : IPaginatedQueryHandler<TQuery, ResultModel<IPaginationResponse<TResponse>>>
    where TQuery : BasePaginatedQuery<TResponse>
{
    protected readonly IUnitOfWork Uow;

    protected BasePaginatedQueryHandler(IUnitOfWork unitOfWork)
    {
        Uow = unitOfWork;
    }
    public abstract Task<ResultModel<IPaginationResponse<TResponse>>> Handle(TQuery request,
        CancellationToken ct = default);
}