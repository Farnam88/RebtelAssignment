using FluentValidation;
using RebtelAssignment.Application.Common.Abstractions.Queries.Pagination;
using RebtelAssignment.Application.Common.DataTransferObjects;
using RebtelAssignment.Application.Core.Insights.DbQueries;
using RebtelAssignment.Application.Core.Insights.Dtos;
using RebtelAssignment.Application.Data;
using RebTelAssignment.Domain.Models;
using RebTelAssignment.Domain.Shared.DataWrapper;

namespace RebtelAssignment.Application.Core.Insights.Queries;

public record TopMembersByLoanQuery(DateTimeRangeDto DateTimeRange) : BasePaginatedQuery<TopMembersByLoanResponseDto>;

public class
    TopMembersByLoanQueryHandler : BasePaginatedQueryHandler<TopMembersByLoanQuery, TopMembersByLoanResponseDto>
{
    public TopMembersByLoanQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<ResultModel<IPaginationResponse<TopMembersByLoanResponseDto>>> Handle(
        TopMembersByLoanQuery request, CancellationToken ct = default)
    {
        var memberRepository = Uow.Repository<Member>();
        var membersResult =
            await memberRepository.ToListAsync(
                new TopMembersByLoanDbQuery(request.DateTimeRange, request.PageNumber, request.PageSize), ct);

        var totalItems = await memberRepository.CountAsync(
            new TopMembersByLoanDbQuery(request.DateTimeRange), ct);

        return new BasePaginationResponse<TopMembersByLoanResponseDto>
        {
            Items = membersResult,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalItems = totalItems
        };
    }
}

public class TopMembersByLoanQueryValidator : AbstractValidator<TopMembersByLoanQuery>
{
    public TopMembersByLoanQueryValidator(IValidator<DateTimeRangeDto> dateTimeRangeValidator)
    {
        RuleFor(r => r.DateTimeRange)
            .SetValidator(dateTimeRangeValidator);

        RuleFor(r => r.PageNumber)
            .GreaterThan(0);

        RuleFor(r => r.PageSize)
            .GreaterThan(0);
    }
}