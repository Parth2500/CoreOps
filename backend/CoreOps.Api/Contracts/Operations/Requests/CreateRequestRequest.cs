namespace CoreOps.Api.Contracts.Operations.Requests;

public sealed record CreateRequestRequest(
    Guid TenantId,
    string Title,
    string? Description,
    Guid WorkflowDefinitionId,
    string InitialStateCode,
    Guid CreatedBy
);
