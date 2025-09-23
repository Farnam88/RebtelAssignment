using FluentValidation;
using RebtelAssignment.Application.Common.Abstractions.Queries.Pagination;
using RebtelAssignment.Application.Common.DataTransferObjects;
using RebtelAssignment.Application.Core.Insights.DbQueries;
using RebtelAssignment.Application.Data;
using RebTelAssignment.Domain.Models;
using RebTelAssignment.Domain.Shared.DataWrapper;

namespace RebtelAssignment.Application.Core.Insights.Queries;

public record LoaningPatternByBookQuery(long BookId) : BasePaginatedQuery<BookDto>;

public class LoaningPatternByBookQueryHandler : BasePaginatedQueryHandler<LoaningPatternByBookQuery, BookDto>
{
    public LoaningPatternByBookQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<ResultModel<IPaginationResponse<BookDto>>> Handle(LoaningPatternByBookQuery request,
        CancellationToken ct = default)
    {
        var bookRepository = Uow.Repository<Book>();

        var books = await bookRepository.ToListAsync(new LoaningPatternByBookDbQuery(request.BookId, request.PageNumber,
            request.PageSize), ct);

        var totalItems = await bookRepository.CountAsync(new LoaningPatternByBookDbQuery(request.BookId), ct);
        return new BasePaginationResponse<BookDto>
        {
            Items = books,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalItems = totalItems
        };
    }
}

public class LoaningPatternByBookQueryValidator : AbstractValidator<LoaningPatternByBookQuery>
{
    public LoaningPatternByBookQueryValidator()
    {
        RuleFor(r => r.BookId).GreaterThan(0);
        RuleFor(r => r.PageNumber)
            .GreaterThan(0);

        RuleFor(r => r.PageSize)
            .GreaterThan(0);
    }
}