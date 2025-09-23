using Ardalis.Specification;
using RebtelAssignment.Application.Common.Abstractions.Specifications;
using RebTelAssignment.Domain.Models;

namespace RebtelAssignment.Application.Core.Loaning.DbQueries;

public class GetBatchesDbQuery : BaseSpec<Batch>
{
    public GetBatchesDbQuery(List<long> batchIds)
    {
        Query.Where(w => batchIds.Contains(w.Id));
    }
}