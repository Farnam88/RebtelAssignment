using Ardalis.Specification;
using RebtelAssignment.Application.Common.Abstractions.Specifications;
using RebTelAssignment.Domain.Models;
using RebTelAssignment.Domain.Models.Enums;
using RebTelAssignment.Domain.Shared.Extensions;

namespace RebtelAssignment.Application.Core.Loaning.RepositorySpecifications;

public class GetMemberUnreturnedLoansSpec : BaseSpec<Loan>
{
    public GetMemberUnreturnedLoansSpec(long memberId)
    {
        Query.Where(w => w.MemberId == memberId &&
                         w.DueAt < DateTime.UtcNow.ToDateOnly() &&
                         w.LoanItems.Any(a => a.ItemStatus == LoanItemStatus.Loaned))
            .AsNoTracking();
    }
}