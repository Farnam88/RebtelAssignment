using FluentValidation;
using RebtelAssignment.Application.Common.Abstractions.Queries;
using RebtelAssignment.Application.Core.Insights.DbQueries;
using RebtelAssignment.Application.Core.Insights.Dtos;
using RebtelAssignment.Application.Core.Services;
using RebtelAssignment.Application.Data;
using RebTelAssignment.Domain.Models;
using RebTelAssignment.Domain.Shared.CustomExceptions;
using RebTelAssignment.Domain.Shared.DataWrapper;

namespace RebtelAssignment.Application.Core.Insights.Queries;

public record MemberReadingPaceQuery(long MemberId) : BaseQuery<MemberReadingPaceResponseDto>;

public class MemberReadingPaceQueryHandler : BaseQueryHandler<MemberReadingPaceQuery, MemberReadingPaceResponseDto>
{
    private readonly IReadingPaceCalculatorService _readingPaceCalculatorService;

    public MemberReadingPaceQueryHandler(IUnitOfWork unitOfWork,
        IReadingPaceCalculatorService readingPaceCalculatorService) : base(unitOfWork)
    {
        _readingPaceCalculatorService = readingPaceCalculatorService;
    }

    public override async Task<ResultModel<MemberReadingPaceResponseDto>> Handle(MemberReadingPaceQuery request,
        CancellationToken ct = default)
    {
        //TODO: check if member exists to have better responses.
        var loanRepository = Uow.Repository<Loan>();
        var loanItemsAggregatedResult =
            await loanRepository.ToListAsync(new MemberLoanItemAggregatedDbQuery(request.MemberId), ct);

        if (!loanItemsAggregatedResult.Any())
            return new NotFoundException("This Member has not been loaning any books");
        var calculatorResult = _readingPaceCalculatorService.CalculatePaceForBook(loanItemsAggregatedResult);
        return new MemberReadingPaceResponseDto
        {
            PagesPerDay = calculatorResult.OverallReadingPace,
            Books = calculatorResult.LoanItems
                .GroupBy(g => new { g.BookId, g.Title })
                .Select(s => new ReadingPaceByBookResponseDto
                {
                    BookId = s.Key.BookId,
                    Title = s.Key.Title,
                    PagesPerDay = s.Sum(d => d.ReadingPace) / s.Count()
                }).ToList()
        };
    }
}

public class MemberReadingPaceQueryValidator : AbstractValidator<MemberReadingPaceQuery>
{
    public MemberReadingPaceQueryValidator()
    {
        RuleFor(r => r.MemberId)
            .GreaterThan(0);
    }
}