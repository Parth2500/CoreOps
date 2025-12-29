using CoreOps.Domain.Workflow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreOps.Infrastructure.Persistence.Configurations.Workflow;

public class WorkflowTransitionConfiguration : IEntityTypeConfiguration<WorkflowTransition>
{
    public void Configure(EntityTypeBuilder<WorkflowTransition> builder)
    {
        builder.ToTable("WorkflowTransitions", "workflow");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.WorkflowDefinitionId).IsRequired();
        builder.Property(x => x.FromStateId).IsRequired();
        builder.Property(x => x.ToStateId).IsRequired();
        builder.Property(x => x.RequiresComment).IsRequired();
        builder.Property(x => x.IsAutomatic).IsRequired();

        builder.HasIndex(x =>new { x.WorkflowDefinitionId, x.FromStateId, x.ToStateId }).IsUnique();
    }
}
