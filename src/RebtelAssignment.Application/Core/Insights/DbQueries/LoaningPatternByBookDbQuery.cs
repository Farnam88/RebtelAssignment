using Ardalis.Specification;
using RebtelAssignment.Application.Common.Abstractions.Specifications;
using RebtelAssignment.Application.Common.DataTransferObjects;
using RebTelAssignment.Domain.Models;

namespace RebtelAssignment.Application.Core.Insights.DbQueries;

public class LoaningPatternByBookDbQuery : BaseSpec<Book, BookDto>
{
    public LoaningPatternByBookDbQuery(long bookId, int pageNumber, int pageSize)
    {
        Query.Where(w => w.LoanItems.Any(a => a.BookId == bookId) && w.Id != bookId);
        Query.Select(a => new BookDto
        {
            Title = a.Title,
            Authors = a.Authors,
            Subjects = a.Subjects,
            Id = a.Id
        });
        Query.Skip((pageNumber - 1) * pageSize);
        Query.Take(pageSize);
    }

    public LoaningPatternByBookDbQuery(long bookId)
    {
        Query.Where(w => w.LoanItems.Any(a => a.BookId == bookId) && w.Id != bookId);
    }
}