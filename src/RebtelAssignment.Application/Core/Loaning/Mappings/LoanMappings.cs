using Mapster;
using RebTelAssignment.Domain.Models;
using RebTelAssignment.Domain.Models.Enums;

namespace RebtelAssignment.Application.Core.Loaning.Mappings;

public class LoanMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Batch, LoanItem>()
            .Map(dest => dest.BatchId, src => src.Id)
            .Map(dest => dest.BookId, src => src.BookId)
            .Map(dest => dest, src => src.BookId)
            .Map(dest => dest.ItemStatus, src => LoanItemStatus.Loaned);
    }
}