using MediatR;
using RebTelAssignment.Domain.Shared.DataWrapper;

namespace RebtelAssignment.Application.Common.Abstractions.Queries.Pagination;

public interface IPaginatedQuery<out TResponse> : IRequest<TResponse>
{
    int PageNumber { get; set; }
    int PageSize { get; set; }
}

public abstract record BasePaginatedQuery<T> : IPaginatedQuery<ResultModel<IPaginationResponse<T>>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}