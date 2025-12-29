using CoreOps.Domain.Common;

namespace CoreOps.Domain.Workflow;

public class WorkflowDefinition : Entity
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    public Guid CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<WorkflowState> _states = new();
    public IReadOnlyCollection<WorkflowState> States => _states;


    private readonly List<WorkflowTransition> _transitions = new();
    public IReadOnlyCollection<WorkflowTransition> Transitions => _transitions;

    private WorkflowDefinition() { } // EF

    public WorkflowDefinition(
        Guid tenantId,
        string name,
        string? description,
        Guid createdBy)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        Name = name;
        Description = description;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
        IsActive = false;
    }
}
