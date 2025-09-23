using FluentValidation;
using RebtelAssignment.Application.Common.Abstractions.Queries.Pagination;
using RebtelAssignment.Application.Common.DataTransferObjects;
using RebtelAssignment.Application.Core.Insights.DbQueries;
using RebtelAssignment.Application.Core.Insights.Dtos;
using RebtelAssignment.Application.Data;
using RebTelAssignment.Domain.Models;
using RebTelAssignment.Domain.Shared.DataWrapper;

namespace RebtelAssignment.Application.Core.Insights.Queries;

public record TopLoanedBookQuery : BasePaginatedQuery<TopLoanedBookResponseDto>;

public class TopLoanedBookQueryHandler : BasePaginatedQueryHandler<TopLoanedBookQuery, TopLoanedBookResponseDto>
{
    public TopLoanedBookQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<ResultModel<IPaginationResponse<TopLoanedBookResponseDto>>> Handle(
        TopLoanedBookQuery request,
        CancellationToken ct = default)
    {
        var bookRepository = Uow.Repository<Book>();
        
        //concurrent asynchronous execution Can be used here 
        var booksResult =
            await bookRepository.ToListAsync(
                new TopLoanedBooksDbQuery(request.PageNumber, request.PageSize), ct);

        var totalItems = await bookRepository.CountAsync(new TopLoanedBooksDbQuery(), ct);
        
        return new BasePaginationResponse<TopLoanedBookResponseDto>
        {
            Items = booksResult,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalItems = totalItems
        };
    }
}

public class TopLoanedBookQueryValidator : AbstractValidator<TopLoanedBookQuery>
{
    public TopLoanedBookQueryValidator(IValidator<DateTimeRangeDto> dateTimeRangeValidator)
    {
        RuleFor(r => r.PageNumber)
            .GreaterThan(0);

        RuleFor(r => r.PageSize)
            .GreaterThan(0);
    }
}