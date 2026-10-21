namespace CoreOps.Application.Workflow.Repositories;

public interface IWorkflowDefinitionRepository
{
    Task<bool> ExistsAsync(Guid workflowDefinitionId, CancellationToken cancellationToken);
}
