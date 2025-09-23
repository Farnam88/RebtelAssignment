using Ardalis.Specification;
using RebtelAssignment.Application.Common.Abstractions.Specifications;
using RebtelAssignment.Application.Common.DataTransferObjects;
using RebTelAssignment.Domain.Models;

namespace RebtelAssignment.Application.Core.CommonDbQueries;

public class GetMemberSpec : BaseSpec<Member, MemberDto>
{
    public GetMemberSpec(long memberId)
    {
        Query.Where(w => w.Id == memberId);
        Query.AsNoTracking();
        Query.Select(s => new MemberDto
        {
            Id = s.Id,
            DisplayName = s.DisplayName
        });
    }
}