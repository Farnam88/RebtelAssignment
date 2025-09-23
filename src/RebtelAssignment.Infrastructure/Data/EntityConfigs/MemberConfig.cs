using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RebTelAssignment.Domain.Models;

namespace RebtelAssignment.Infrastructure.Data.EntityConfigs;

public class MemberConfig : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(k => k.Id);

        builder.Property(p => p.DisplayName)
            .IsRequired();
    }
}