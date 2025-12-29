using CoreOps.Api.Contracts.Operations.Requests;
using CoreOps.Application.Operations.CreateRequest;
using Microsoft.AspNetCore.Mvc;

namespace CoreOps.Api.Controllers.Operations;

[ApiController]
[Route("api/[controller]")]
public sealed class RequestsController : ControllerBase
{
    private readonly CreateRequestHandler _handler;

    public RequestsController(CreateRequestHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<ActionResult<CreateRequestResponse>> Create(
        [FromBody] CreateRequestRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateRequestCommand(
            TenantId: request.TenantId,
            Title: request.Title,
            Description: request.Description,
            WorkflowDefinitionId: request.WorkflowDefinitionId,
            InitialStateCode: request.InitialStateCode,
            CreatedBy: request.CreatedBy
        );

        var result = await _handler.Handle(command, cancellationToken);

        return Ok(new CreateRequestResponse(result.RequestId));
    }
}
