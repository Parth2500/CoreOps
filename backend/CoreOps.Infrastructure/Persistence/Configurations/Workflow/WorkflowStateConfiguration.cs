using CoreOps.Domain.Workflow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreOps.Infrastructure.Persistence.Configurations.Workflow;

public class WorkflowStateConfiguration : IEntityTypeConfiguration<WorkflowState>
{
    public void Configure(EntityTypeBuilder<WorkflowState> builder)
    {
        builder.ToTable("WorkflowStates", "workflow");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.WorkflowDefinitionId).IsRequired();
        builder.Property(x => x.Code).IsRequired().HasMaxLength(100);
        builder.Property(x => x.DisplayName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.IsInitial).IsRequired();
        builder.Property(x => x.IsTerminal).IsRequired();
        builder.Property(x => x.SortOrder).IsRequired();

        builder.HasIndex(x => new { x.WorkflowDefinitionId, x.Code }).IsUnique();
    }
}
