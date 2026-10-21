using CoreOps.Application.Workflow.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoreOps.Infrastructure.Persistence.Repositories.Workflow;

internal class WorkflowDefinitionRepository : IWorkflowDefinitionRepository
{
    private readonly CoreOpsDbContext _dbContext;

    public WorkflowDefinitionRepository(CoreOpsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExistsAsync(
        Guid workflowDefinitionId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.WorkflowDefinitions
            .AnyAsync(
                w => w.Id == workflowDefinitionId,
                cancellationToken);
    }
}
