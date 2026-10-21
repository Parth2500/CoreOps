using CoreOps.Api.Contracts.Operations.Requests;
using CoreOps.Application.Operations.CreateRequest;
using CoreOps.Application.Operations.GetRequest;
using Microsoft.AspNetCore.Mvc;

namespace CoreOps.Api.Controllers.Operations;

[ApiController]
[Route("api/[controller]")]
public sealed class RequestsController : ControllerBase
{
    private readonly CreateRequestHandler _handler;
    private readonly GetRequestByIdQueryHandler _getRequestByIdHandler;

    public RequestsController(
        CreateRequestHandler handler, 
        GetRequestByIdQueryHandler getRequestByIdHandler
    ) {
        _handler = handler;
        _getRequestByIdHandler = getRequestByIdHandler;
    }

    [HttpPost]
    public async Task<ActionResult<CreateRequestResponse>> Create(
        [FromBody] CreateRequestRequest request,
        CancellationToken cancellationToken
    ) {
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

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetRequestResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken
    ) {
        var result = await _getRequestByIdHandler.Handle(new GetRequestByIdQuery(id), cancellationToken);

        var response = new GetRequestResponse(
            result.Id,
            result.TenantId,
            result.Title,
            result.Description,
            result.WorkflowDefinitionId,
            result.CurrentStateCode,
            result.CreatedBy,
            result.CreatedAt,
            result.LastModifiedAt);

        return Ok(response);
    }
}
