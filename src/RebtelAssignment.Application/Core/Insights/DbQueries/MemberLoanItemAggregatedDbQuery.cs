using Ardalis.Specification;
using RebtelAssignment.Application.Common.Abstractions.Specifications;
using RebtelAssignment.Application.Core.Insights.Queries;
using RebtelAssignment.Application.Core.Services;
using RebTelAssignment.Domain.Models;
using RebTelAssignment.Domain.Models.Enums;

namespace RebtelAssignment.Application.Core.Insights.DbQueries;

public class MemberLoanItemAggregatedDbQuery : BaseSpec<Loan, LoanItemAggregatedModel>
{
    public MemberLoanItemAggregatedDbQuery(long memberId)
    {
        Query.Where(w =>
            w.MemberId == memberId &&
            w.LoanItems.Any(a => a.ItemStatus != LoanItemStatus.Loaned && a.ReturnedAt != null));
        Query.SelectMany(s => s.LoanItems.Select(d => new LoanItemAggregatedModel
        {
            BookId = d.BookId,
            BatchId = d.BatchId,
            Title = d.Book.Title,
            LoanedAt = s.LoanedAt,
            ReturnedAt = d.ReturnedAt,
            Pages = d.Batch.Pages
        }));
    }
}