using CoreOps.Domain.Workflow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreOps.Infrastructure.Persistence.Configurations.Workflow;

public class WorkflowDefinitionConfiguration : IEntityTypeConfiguration<WorkflowDefinition>
{
    public void Configure(EntityTypeBuilder<WorkflowDefinition> builder)
    {
        builder.ToTable("WorkflowDefinitions", "workflow");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.IsActive).IsRequired();

        builder.Property(x => x.CreatedBy).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasMany(x => x.States)
               .WithOne()
               .HasForeignKey(x => x.WorkflowDefinitionId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Transitions)
               .WithOne()
               .HasForeignKey(x => x.WorkflowDefinitionId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
