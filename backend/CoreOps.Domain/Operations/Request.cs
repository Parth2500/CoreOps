using CoreOps.Domain.Common;

namespace CoreOps.Domain.Operations;

public class Request : Entity
{
    public string Title { get; private set; } = default!;
    public string? Description { get; private set; }
    public Guid WorkflowDefinitionId { get; private set; }
    public string CurrentStateCode { get; private set; } = default!;
    public Guid CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime LastModifiedAt { get; private set; }

    private Request() { }

    public Request(
        Guid tenantId,
        string title,
        string? description,
        Guid workflowDefinitionId,
        string initialStateCode,
        Guid createdBy)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        Title = title;
        Description = description;
        WorkflowDefinitionId = workflowDefinitionId;
        CurrentStateCode = initialStateCode;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
        LastModifiedAt = CreatedAt;
    }
}
