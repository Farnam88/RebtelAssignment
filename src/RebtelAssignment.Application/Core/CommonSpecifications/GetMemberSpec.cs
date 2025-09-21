using Ardalis.Specification;
using RebtelAssignment.Application.Common.Abstractions.Specifications;
using RebtelAssignment.Application.Common.DataTransferObjects;
using RebTelAssignment.Domain.Models;

namespace RebtelAssignment.Application.Core.CommonSpecifications;

public class GetMemberSpec : BaseSpec<Member, MemberDto>
{
    public GetMemberSpec(long memberId)
    {
        Query.Where(w => w.Id == memberId);
        Query.AsNoTracking();
        Query.Select(s => new MemberDto
        {
            Id = s.Id,
            Email = s.Email,
            PhoneNumber = s.PhoneNumber,
            DisplayName = s.DisplayName
        });
    }
}