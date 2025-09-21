using Ardalis.Specification;
using RebtelAssignment.Application.Common.Abstractions.Specifications;
using RebtelAssignment.Application.Common.DataTransferObjects;
using RebTelAssignment.Domain.Models;

namespace RebtelAssignment.Application.Core.Loaning.RepositorySpecifications;

public class GetBatchesSpec : BaseSpec<Batch>
{
    public GetBatchesSpec(List<long> batchIds)
    {
        Query.Where(w => batchIds.Contains(w.Id));
        Query.Include(i => i.InventoryItem);
    }
}