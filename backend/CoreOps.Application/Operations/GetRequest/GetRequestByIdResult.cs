using CoreOps.Domain.Operations;

namespace CoreOps.Application.Operations.GetRequest;

public record GetRequestByIdResult(
    Guid Id,
    Guid TenantId,
    string Title,
    string? Description,
    Guid WorkflowDefinitionId,
    string CurrentStateCode,
    Guid CreatedBy,
    DateTime CreatedAt,
    DateTime LastModifiedAt);
