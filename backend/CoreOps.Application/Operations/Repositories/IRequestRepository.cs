using CoreOps.Domain.Operations;

namespace CoreOps.Application.Operations.Repositories;

public interface IRequestRepository
{
    Task AddAsync(Request request, CancellationToken cancellationToken);
}
