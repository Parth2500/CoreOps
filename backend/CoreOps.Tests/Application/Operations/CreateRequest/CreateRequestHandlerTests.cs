using CoreOps.Application.Common.Validation;
using CoreOps.Application.Operations.CreateRequest;
using CoreOps.Application.Operations.Repositories;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreOps.Tests.Application.Operations.CreateRequest;

public sealed class CreateRequestHandlerTests
{
    private readonly Mock<IRequestRepository> _requestRepositoryMock;
    private readonly CreateRequestHandler _handler;

    public CreateRequestHandlerTests()
    {
        _requestRepositoryMock = new Mock<IRequestRepository>();
        _handler = new CreateRequestHandler(_requestRepositoryMock.Object);
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
}
