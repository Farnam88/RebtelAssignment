using Ardalis.Specification;
using RebtelAssignment.Application.Common.Abstractions.Specifications;
using RebtelAssignment.Application.Common.DataTransferObjects;
using RebTelAssignment.Domain.Models;

namespace RebtelAssignment.Application.Core.Loaning.RepositorySpecifications;

public class GetBatchesSpec : BaseSpec<Batch, BatchDto>
{
    public GetBatchesSpec(List<long> batchIds)
    {
        Query.Where(w => batchIds.Contains(w.Id));
        Query.Include(i => i.InventoryItem);
        Query.Select(s => new BatchDto
        {
            Id = s.Id,
            Isbn = s.Isbn,
            Pages = s.Pages,
            Publisher = s.Publisher,
            BookId = s.BookId,
            Edition = s.Edition,
            PublishedDate = s.PublishedDate,
            AvailableQuantity = s.InventoryItem.QuantityAvailable
        });
    }
}