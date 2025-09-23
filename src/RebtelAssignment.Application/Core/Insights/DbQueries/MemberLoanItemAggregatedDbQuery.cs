using Ardalis.Specification;
using RebtelAssignment.Application.Common.Abstractions.Specifications;
using RebtelAssignment.Application.Core.Insights.Queries;
using RebtelAssignment.Application.Core.Services;
using RebTelAssignment.Domain.Models;
using RebTelAssignment.Domain.Models.Enums;

namespace RebtelAssignment.Application.Core.Insights.DbQueries;

public class MemberLoanItemAggregatedDbQuery : BaseSpec<LoanItem, LoanItemAggregatedModel>
{
    public MemberLoanItemAggregatedDbQuery(long memberId)
    {
        Query.Where(w =>
            w.Loan.MemberId == memberId && w.ItemStatus != LoanItemStatus.Loaned && w.ReturnedAt != null);
        Query.Select(s => new LoanItemAggregatedModel
        {
            BookId = s.BookId,
            BatchId = s.BatchId,
            Title = s.Book.Title,
            LoanedAt = s.Loan.LoanedAt,
            ReturnedAt = s.ReturnedAt,
            Pages = s.Batch.Pages
        });
    }
}