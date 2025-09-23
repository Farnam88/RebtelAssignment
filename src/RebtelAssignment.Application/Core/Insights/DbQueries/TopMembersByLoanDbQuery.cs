using Ardalis.Specification;
using RebtelAssignment.Application.Common.Abstractions.Specifications;
using RebtelAssignment.Application.Common.DataTransferObjects;
using RebtelAssignment.Application.Core.Insights.Dtos;
using RebTelAssignment.Domain.Models;

namespace RebtelAssignment.Application.Core.Insights.DbQueries;

public class TopMembersByLoanDbQuery : BaseSpec<Member, TopMembersByLoanResponseDto>
{
    public TopMembersByLoanDbQuery(DateTimeRangeDto dateTimeRange, int pageNumber, int pageSize)
    {
        Query.Where(w =>
            Enumerable.Any<Loan>(w.Loans, a => a.LoanedAt >= dateTimeRange.From && a.LoanedAt <= dateTimeRange.To));

        Query.OrderByDescending(s =>
            s.Loans.Count(c => c.LoanedAt >= dateTimeRange.From && c.LoanedAt <= dateTimeRange.To));

        Query.Select(s => new TopMembersByLoanResponseDto
        {
            MemberId = s.Id,
            DisplayName = s.DisplayName,
            LoanCount = s.Loans.Count(c => c.LoanedAt >= dateTimeRange.From && c.LoanedAt <= dateTimeRange.To)
        });

        Query.Skip((pageNumber - 1) * pageSize);
        Query.Take(pageSize);
    }

    /// <summary>
    /// Count query
    /// </summary>
    /// <param name="dateTimeRange"></param>
    public TopMembersByLoanDbQuery(DateTimeRangeDto dateTimeRange)
    {
        Query.Where(w => w.Loans.Any(a => a.LoanedAt >= dateTimeRange.From && a.LoanedAt <= dateTimeRange.To));
    }
}