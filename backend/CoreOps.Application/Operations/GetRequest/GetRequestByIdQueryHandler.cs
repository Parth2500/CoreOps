using CoreOps.Application.Common.Exceptions;
using CoreOps.Application.Operations.Repositories;

namespace CoreOps.Application.Operations.GetRequest;

public sealed class GetRequestByIdQueryHandler
{
    private readonly IRequestRepository _requestRepository;

    public GetRequestByIdQueryHandler(IRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public async Task<GetRequestByIdResult> Handle(GetRequestByIdQuery request, CancellationToken cancellationToken)
    {
        var existingRequest = await _requestRepository.GetByIdAsync(request.RequestId, cancellationToken);

        if (existingRequest is null)
        {
            throw new NotFoundException($"Request with ID '{request.RequestId}' was not found.");
        }

        return new GetRequestByIdResult(
            existingRequest.Id,
            existingRequest.TenantId,
            existingRequest.Title,
            existingRequest.Description,
            existingRequest.WorkflowDefinitionId,
            existingRequest.CurrentStateCode,
            existingRequest.CreatedBy,
            existingRequest.CreatedAt,
            existingRequest.LastModifiedAt);
    }
}
