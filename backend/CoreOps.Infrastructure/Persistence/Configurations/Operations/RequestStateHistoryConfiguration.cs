using CoreOps.Domain.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreOps.Infrastructure.Persistence.Configurations.Operations;

public class RequestStateHistoryConfiguration : IEntityTypeConfiguration<RequestStateHistory>
{
    public void Configure(EntityTypeBuilder<RequestStateHistory> builder)
    {
        builder.ToTable("RequestStateHistory", "operations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.RequestId).IsRequired();
        builder.Property(x => x.FromStateCode).HasMaxLength(100);
        builder.Property(x => x.ToStateCode).IsRequired().HasMaxLength(100);
        builder.Property(x => x.TransitionedBy).IsRequired();
        builder.Property(x => x.TransitionedAt).IsRequired();
        builder.Property(x => x.Comment).HasMaxLength(500);

        builder.HasIndex(x => new { x.RequestId, x.TransitionedAt });
    }
}
