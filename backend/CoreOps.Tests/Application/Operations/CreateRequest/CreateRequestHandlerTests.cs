using CoreOps.Application.Common.Exceptions;
using CoreOps.Application.Common.Validation;
using CoreOps.Application.Operations.CreateRequest;
using CoreOps.Application.Operations.Repositories;
using CoreOps.Application.Workflow.Repositories;
using FluentAssertions;
using Moq;

namespace CoreOps.Tests.Application.Operations.CreateRequest;

public sealed class CreateRequestHandlerTests
{
    private readonly Mock<IRequestRepository> _requestRepositoryMock;
    private readonly Mock<IWorkflowDefinitionRepository> _workflowRepositoryMock;
    private readonly CreateRequestHandler _handler;

    public CreateRequestHandlerTests()
    {
        _requestRepositoryMock = new Mock<IRequestRepository>();
        _workflowRepositoryMock = new Mock<IWorkflowDefinitionRepository>();
        _handler = new CreateRequestHandler(_requestRepositoryMock.Object, _workflowRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_Request_When_Command_Is_Valid()
    {
        // Arrange
        var command = new CreateRequestCommand(
            TenantId: Guid.NewGuid(),
            Title: "Valid title",
            Description: "Some description",
            WorkflowDefinitionId: Guid.NewGuid(),
            InitialStateCode: "Draft",
            CreatedBy: Guid.NewGuid()
        );

        _workflowRepositoryMock
            .Setup(r => r.ExistsAsync(command.WorkflowDefinitionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.RequestId.Should().NotBe(Guid.Empty);

        _requestRepositoryMock.Verify(
            repo => repo.AddAsync(
                It.IsAny<CoreOps.Domain.Operations.Request>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_ValidationException_When_Command_Is_Invalid()
    {
        // Arrange
        var invalidCommand = new CreateRequestCommand(
            TenantId: Guid.Empty, // invalid
            Title: "",
            Description: null,
            WorkflowDefinitionId: Guid.Empty,
            InitialStateCode: "",
            CreatedBy: Guid.Empty
        );

        // Act
        Func<Task> act = async () =>
            await _handler.Handle(invalidCommand, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();

        _requestRepositoryMock.Verify(
            repo => repo.AddAsync(
                It.IsAny<CoreOps.Domain.Operations.Request>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_Workflow_Does_Not_Exist()
    {
        // Arrange
        var command = new CreateRequestCommand(
            Guid.NewGuid(),
            "Title",
            null,
            Guid.NewGuid(),
            "Draft",
            Guid.NewGuid());

        _workflowRepositoryMock
            .Setup(r => r.ExistsAsync(
                command.WorkflowDefinitionId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        Func<Task> act = async () =>
            await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();

        _requestRepositoryMock.Verify(
            r => r.AddAsync(
                It.IsAny<CoreOps.Domain.Operations.Request>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
