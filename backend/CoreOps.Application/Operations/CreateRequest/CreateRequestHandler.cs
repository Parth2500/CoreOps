using CoreOps.Application.Operations.Repositories;
using CoreOps.Domain.Operations;
using CoreOps.Application.Common.Validation;

namespace CoreOps.Application.Operations.CreateRequest;

public sealed class CreateRequestHandler
{
    private readonly IRequestRepository _requestRepository;

    public CreateRequestHandler(IRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public async Task<CreateRequestResult> Handle(
        CreateRequestCommand command,
        CancellationToken cancellationToken)
    {
        CreateRequestValidator.Validate(command);

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
