using CoreOps.Domain.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreOps.Infrastructure.Persistence.Configurations.Audit;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs", "audit");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.EntityType).IsRequired().HasMaxLength(100);
        builder.Property(x => x.EntityId).IsRequired();
        builder.Property(x => x.Action).IsRequired().HasMaxLength(100);
        builder.Property(x => x.PerformedBy).IsRequired();
        builder.Property(x => x.PerformedAt).IsRequired();
        builder.Property(x => x.Data).IsRequired();
    }
}
