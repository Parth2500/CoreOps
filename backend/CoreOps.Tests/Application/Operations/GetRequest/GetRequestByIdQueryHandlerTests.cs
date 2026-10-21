using CoreOps.Application.Common.Exceptions;
using CoreOps.Application.Operations.GetRequest;
using CoreOps.Application.Operations.Repositories;
using CoreOps.Domain.Operations;
using FluentAssertions;
using Moq;

namespace CoreOps.Tests.Application.Operations.GetRequest;

public sealed class GetRequestByIdQueryHandlerTests
{
    private readonly Mock<IRequestRepository> _requestRepositoryMock;
    private readonly GetRequestByIdQueryHandler _handler;

    public GetRequestByIdQueryHandlerTests()
    {
        _requestRepositoryMock = new Mock<IRequestRepository>();
        _handler = new GetRequestByIdQueryHandler(_requestRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_GetRequestByIdResult_When_Request_Exists()
    {
        // Arrange
        var requestId = Guid.NewGuid();
        var request = new Request(
            Guid.NewGuid(),
            "Title",
            "Description",
            Guid.NewGuid(),
            "Draft",
            Guid.NewGuid());
        
        // Reflection to set the ID since it's private set in constructor
        typeof(Request).GetProperty("Id")!.SetValue(request, requestId);

        _requestRepositoryMock
            .Setup(r => r.GetByIdAsync(requestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(request);

        var query = new GetRequestByIdQuery(requestId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(requestId);
        result.Title.Should().Be(request.Title);
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_Request_Does_Not_Exist()
    {
        // Arrange
        var requestId = Guid.NewGuid();
        
        _requestRepositoryMock
            .Setup(r => r.GetByIdAsync(requestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Request?)null);

        var query = new GetRequestByIdQuery(requestId);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Request with ID '{requestId}' was not found.");
    }
}
