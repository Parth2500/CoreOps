namespace CoreOps.Api.Contracts.Operations.Requests;

public record GetRequestResponse(
    Guid Id,
    Guid TenantId,
    string Title,
    string? Description,
    Guid WorkflowDefinitionId,
    string CurrentStateCode,
    Guid CreatedBy,
    DateTime CreatedAt,
    DateTime LastModifiedAt);
