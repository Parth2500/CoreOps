using CoreOps.Application.Operations.Repositories;
using CoreOps.Domain.Operations;
using Microsoft.EntityFrameworkCore;

namespace CoreOps.Infrastructure.Persistence.Repositories.Operations;

public sealed class RequestRepository : IRequestRepository
{
    private readonly CoreOpsDbContext _dbContext;

    public RequestRepository(CoreOpsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(
        Request request,
        CancellationToken cancellationToken)
    {
        await _dbContext.Requests.AddAsync(request, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Request?> GetByIdAsync(Guid requestId, CancellationToken cancellationToken)
    {
        return await _dbContext.Requests
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken);
    }
}
