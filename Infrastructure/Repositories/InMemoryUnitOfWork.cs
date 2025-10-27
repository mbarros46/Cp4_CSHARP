using Domain.Repositories;

namespace Infrastructure.Repositories;

public class InMemoryUnitOfWork : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct = default) => Task.FromResult(0);
}
