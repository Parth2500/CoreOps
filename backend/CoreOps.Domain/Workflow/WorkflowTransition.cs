using CoreOps.Domain.Common;

namespace CoreOps.Domain.Workflow;

public class WorkflowTransition : Entity
{
    public Guid WorkflowDefinitionId { get; private set; }
    public Guid FromStateId { get; private set; }
    public Guid ToStateId { get; private set; }
    public bool RequiresComment { get; private set; }
    public bool IsAutomatic { get; private set; }

    private WorkflowTransition() { }

    public WorkflowTransition(
        Guid workflowDefinitionId,
        Guid fromStateId,
        Guid toStateId,
        bool requiresComment,
        bool isAutomatic)
    {
        Id = Guid.NewGuid();
        WorkflowDefinitionId = workflowDefinitionId;
        FromStateId = fromStateId;
        ToStateId = toStateId;
        RequiresComment = requiresComment;
        IsAutomatic = isAutomatic;
    }
}
