using Ardalis.Specification;
using RebtelAssignment.Application.Common.Abstractions.Specifications;
using RebTelAssignment.Domain.Models;

namespace RebtelAssignment.Application.Core.CommonSpecifications;

public class GetActiveLoanSettingSpec : BaseSpec<LoanSetting>
{
    public GetActiveLoanSettingSpec()
    {
        Query.Where(w => w.IsActive);
        Query.AsNoTracking();
    }
}