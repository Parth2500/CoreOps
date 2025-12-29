using CoreOps.Domain.Common;

namespace CoreOps.Domain.Workflow;

public class WorkflowState : Entity
{
    public Guid WorkflowDefinitionId { get; private set; }
    public string Code { get; private set; } = default!;
    public string DisplayName { get; private set; } = default!;
    public bool IsInitial { get; private set; }
    public bool IsTerminal { get; private set; }
    public int SortOrder { get; private set; }

    private WorkflowState() { }

    public WorkflowState(
        Guid workflowDefinitionId,
        string code,
        string displayName,
        bool isInitial,
        bool isTerminal,
        int sortOrder)
    {
        Id = Guid.NewGuid();
        WorkflowDefinitionId = workflowDefinitionId;
        Code = code;
        DisplayName = displayName;
        IsInitial = isInitial;
        IsTerminal = isTerminal;
        SortOrder = sortOrder;
    }
}
