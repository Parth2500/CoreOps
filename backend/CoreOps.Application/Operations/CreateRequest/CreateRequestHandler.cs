using CoreOps.Application.Common.Exceptions;
using CoreOps.Application.Common.Validation;
using CoreOps.Application.Operations.Repositories;
using CoreOps.Application.Workflow.Repositories;
using CoreOps.Domain.Operations;

namespace CoreOps.Application.Operations.CreateRequest;

public sealed class CreateRequestHandler
{
    private readonly IRequestRepository _requestRepository;
    private readonly IWorkflowDefinitionRepository _workflowRepository;

    public CreateRequestHandler(IRequestRepository requestRepository, IWorkflowDefinitionRepository workflowRepository)
    {
        _requestRepository = requestRepository;
        _workflowRepository = workflowRepository;
    }

    public async Task<CreateRequestResult> Handle(
        CreateRequestCommand command,
        CancellationToken cancellationToken)
    {
        CreateRequestValidator.Validate(command);

        var workflowExists = await _workflowRepository.ExistsAsync(command.WorkflowDefinitionId, cancellationToken);

        if (!workflowExists)
        {
            throw new NotFoundException($"WorkflowDefinition '{command.WorkflowDefinitionId}' does not exist.");
        }

        var request = new Request(
            tenantId: command.TenantId,
            title: command.Title,
            description: command.Description,
            workflowDefinitionId: command.WorkflowDefinitionId,
            initialStateCode: command.InitialStateCode,
            createdBy: command.CreatedBy
        );

        await _requestRepository.AddAsync(request, cancellationToken);

        return new CreateRequestResult(request.Id);
    }
}
