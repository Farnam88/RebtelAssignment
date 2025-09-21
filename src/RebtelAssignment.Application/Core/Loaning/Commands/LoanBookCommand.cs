using FluentValidation;
using RebtelAssignment.Application.Common.Abstractions.Commands;
using RebtelAssignment.Application.Common.DataTransferObjects;
using RebtelAssignment.Application.Core.Loaning.RepositorySpecifications;
using RebtelAssignment.Application.Data;
using RebTelAssignment.Domain.Models;
using RebTelAssignment.Domain.Shared.CustomExceptions;
using RebTelAssignment.Domain.Shared.DataWrapper;

namespace RebtelAssignment.Application.Core.Loaning.Commands;

public class LoanBookCommand : BaseCommand<LoanDto>
{
    public required List<long> BatchIds { get; set; }
    public long MemberId { get; set; }
}

public class LoanBookCommandHandler : BaseCommandHandler<LoanBookCommand, LoanDto>
{
    public LoanBookCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<ResultModel<LoanDto>> Handle(LoanBookCommand request, CancellationToken ct = default)
    {
        var batchRepo = Uow.Repository<Batch>();
        var batchList = await batchRepo.ToListAsync(new GetBatchesSpec(request.BatchIds), ct);
        //for simplicity, otherwise the result should indicate which batch Id was not found
        if (request.BatchIds.Count != batchList.Count)
            return new NotFoundException("Batch not found");

        return null!;
    }
}

public class LoanBookCommandValidator : AbstractValidator<LoanBookCommand>
{
    public LoanBookCommandValidator()
    {
        RuleFor(r => r.BatchIds).NotEmpty();
        RuleFor(r => r.MemberId).GreaterThan(0);
    }
}