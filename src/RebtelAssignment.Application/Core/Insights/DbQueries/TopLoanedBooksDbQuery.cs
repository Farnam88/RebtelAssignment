using Ardalis.Specification;
using RebtelAssignment.Application.Common.Abstractions.Specifications;
using RebtelAssignment.Application.Core.Insights.Dtos;
using RebTelAssignment.Domain.Models;

namespace RebtelAssignment.Application.Core.Insights.DbQueries;

public class TopLoanedBooksDbQuery : BaseSpec<Book, TopLoanedBookResponseDto>
{
    public TopLoanedBooksDbQuery(int pageNumber, int pageSize)
    {
        Query.Select(s => new TopLoanedBookResponseDto
        {
            Title = s.Title,
            BookId = s.Id,
            Count = s.LoanItems.Count
        });
        Query.OrderByDescending(o => o.LoanItems.Count);
        Query.Skip((pageNumber - 1) * pageSize);
        Query.Take(pageSize);
    }

    /// <summary>
    /// Count Query
    /// </summary>
    public TopLoanedBooksDbQuery()
    {
    }
}