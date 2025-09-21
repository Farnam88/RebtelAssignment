using FluentValidation;
using MapsterMapper;
using RebtelAssignment.Application.Common.Abstractions.Commands;
using RebtelAssignment.Application.Core.CommonSpecifications;
using RebtelAssignment.Application.Core.Loaning.RepositorySpecifications;
using RebtelAssignment.Application.Data;
using RebTelAssignment.Domain.Models;
using RebTelAssignment.Domain.Models.Enums;
using RebTelAssignment.Domain.Shared.CustomExceptions;
using RebTelAssignment.Domain.Shared.DataWrapper;
using RebTelAssignment.Domain.Shared.Extensions;

namespace RebtelAssignment.Application.Core.Loaning.Commands;

public class LoanBookCommand : BaseCommand<Success>
{
    public required List<long> BatchIds { get; set; }
    public long MemberId { get; set; }
}

public class LoanBookCommandHandler : BaseCommandHandler<LoanBookCommand, Success>
{
    private readonly ILoanDueDateCalculatorService _loanDueDateCalculatorService;
    private readonly IMapper _mapper;

    public LoanBookCommandHandler(IUnitOfWork unitOfWork, ILoanDueDateCalculatorService loanDueDateCalculatorService,
        IMapper mapper) :
        base(unitOfWork)
    {
        _loanDueDateCalculatorService = loanDueDateCalculatorService;
        _mapper = mapper;
    }

    public override async Task<ResultModel<Success>> Handle(LoanBookCommand request, CancellationToken ct = default)
    {
        var batchRepo = Uow.Repository<Batch>();
        var batchList = await batchRepo.ToListAsync(new GetBatchesSpec(request.BatchIds), ct);
        //for simplicity, otherwise the result should indicate which batch Id was not found
        if (request.BatchIds.Count != batchList.Count)
            return new NotFoundException("Batch not found");

        if (batchList.Any(a => a.InventoryItem.QuantityAvailable == 0))
            return new BusinessLogicException("Batch has no available quantity");

        //TODO: Need to implement cache
        var memberRepo = Uow.Repository<Member>();
        var member = await memberRepo.FirstOrDefaultAsync(new GetMemberSpec(request.MemberId), ct);
        if (member == null)
            return new NotFoundException("Member not found");

        //TODO: Need to implement cache
        var loanSettingRepo = Uow.Repository<LoanSetting>();
        var loanSetting = await loanSettingRepo.FirstOrDefaultAsync(new GetActiveLoanSettingSpec(), ct);
        if (loanSetting == null)
            return new InternalServiceException("LoanSetting is not set");

        var loanRepo = Uow.Repository<Loan>();

        if (await loanRepo.AnyAsync(new GetMemberUnreturnedLoansSpec(request.MemberId), ct))
            return new BusinessLogicException("The member has an unreturned loan that is passed is due date");

        //TODO: mapper
        DateTime loanDate = DateTime.UtcNow;
        var loan = new Loan
        {
            MemberId = request.MemberId,
            LoanedAt = loanDate.ToDateOnly(),
            DueAt = _loanDueDateCalculatorService.CalculateLoanDueDate(loanSetting, loanDate),
            LoanItems = batchList.Select(s=>new LoanItem
            {
                BatchId = s.Id,
                ItemStatus = LoanItemStatus.Loaned,
                BookId = s.BookId,
            }).ToList()
        };
        
        await loanRepo.AddAsync(loan, ct);

        foreach (var loanItem in loan.LoanItems)
        {
            var batch = batchList.First(f => f.Id == loanItem.BatchId);
            batch.InventoryItem.QuantityLoaned += 1;
        }

        await Uow.CommitAsync(ct);

        //TODO: Publish Loan Created event
        return Success.Created;
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