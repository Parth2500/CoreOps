namespace CoreOps.Application.Operations.CreateRequest;

public sealed record CreateRequestCommand(
    Guid TenantId,
    string Title,
    string? Description,
    Guid WorkflowDefinitionId,
    string InitialStateCode,
    Guid CreatedBy
);
